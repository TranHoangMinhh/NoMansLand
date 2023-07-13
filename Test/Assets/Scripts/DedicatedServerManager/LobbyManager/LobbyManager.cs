using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using System;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    //! Add key for each DataObject
    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_SIDE_WEAPON = "SideWeapon";
    public const string KEY_PLAYER_CHARACTER = "Character";

    public const string KEY_DURATION = "Duration";
    public const string KEY_MAP = "Map";
    public const string KEY_MAP_WEATHER = "Map_Weather";

    // Event publishers for each action on lobby
    public event EventHandler OnEnterGame;
    public event EventHandler OnLeaveGame;

    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }


    // Enum list of characters
    public enum PlayerCharacter
    {
        None,
        Daughter_01,
        Father_01,
        Father_02,
        Mother_01,
        Mother_02,
        SchoolBoy_01,
        SchoolGirl_01,
        ShopKeeper_01,
        Son_01
    }

    // Enum list of side weapons
    public enum SideWeapon
    {
        None,
        Knife,
        Pistol,
        Revolver,
        Grenade,
        Smoke
    }

    // Enum list of maps
    public enum Maps
    {
        TheTown,
        GasStation,
        LoneIsland
    }

    public int menuBackgroundIndex {  get; set; }
    public int lobbyBackgroundIndex { get; set; }

    // Enum list of map weathers
    public enum Map_Weathers
    {
        Sunny,
        Rain,
        Fog
    }


    private string _playerID;
    private string _playerName;
    private float _heartbeatTimer;
    private float _lobbyUpdateTimer;

    private Lobby _hostLobby;
    private Lobby _joinedLobby;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
    }

    public string GetPlayerID()
    {
        return _playerID;
    }

    public Lobby GetJoinedLobby()
    {
        return _joinedLobby;
    }

    public async void Authenticate(string playerName)
    {
        //! Change to player's input name. Check on DB using playerID:
        // - if exist -> get player name
        // - if not exist -> add player to DB
        _playerName = playerName;

        // Check if Unity Services have been initialized
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            //! Remove this on main build
            // Set profile if need to sign in multiple account on single device.
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(playerName);

            await UnityServices.InitializeAsync(initializationOptions);
        }

        AuthenticationService.Instance.SignedIn += () =>
        {
            _playerID = AuthenticationService.Instance.PlayerId;

#if UNITY_EDITOR
            Debug.Log($"Signed in successful. Player ID: {_playerID}; Player name: {_playerName}");
#endif

            //! Add update DB on player information (ID + name)
        };

        // Sign in player async
        // No need to check if token exist, because it will automatically use one if existed
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Send a ping every 30 seconds to UGS Lobby to keep it alive
    // Check out https://docs.unity.com/lobby/en-us/manual/heartbeat-a-lobby
    private async void HandleLobbyHeartbeat()
    {
        if (_hostLobby != null)
        {
            _heartbeatTimer -= Time.deltaTime;

            if (_heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 30f;
                _heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
    }

    // Update lobby every 2 seconds (considered to the limit of poll) to synchronize changes made by other player(s)
    // Check out https://docs.unity.com/lobby/en-us/manual/poll-for-updates
    private async void HandleLobbyPolling()
    {
        if (_joinedLobby != null)
        {
            _lobbyUpdateTimer -= Time.deltaTime;
            if (_lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                _lobbyUpdateTimer = lobbyUpdateTimerMax;

                _joinedLobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);

                // Call event to update lobby
                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby =  _joinedLobby });

                if (!IsPlayerInLobby())
                {
                    // Player was kicked out of this lobby
                    Debug.Log("Kicked from Lobby!");

                    OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = _joinedLobby });

                    _joinedLobby = null;
                }
            }
        }
    }

    public bool IsLobbyHost()
    {
        return _joinedLobby != null && _joinedLobby.HostId == _playerID;
    }

    private bool IsPlayerInLobby()
    {
        if (_joinedLobby != null && _joinedLobby.Players != null)
        {
            foreach (Player player in _joinedLobby.Players)
            {
                if (player.Id == _playerID) return true;
            }
        }

        return false;
    }

    public void DisableOnEnterGame()
    {
        gameObject.SetActive(false);
        OnEnterGame?.Invoke(this, EventArgs.Empty);

#if UNITY_EDITOR
        Debug.Log("Enter Game - Disable Lobby Manager!!!");
#endif
    }

    public void EnableOnExitGame()
    {
        gameObject.SetActive(true);
        OnLeaveGame?.Invoke(this, EventArgs.Empty);

#if UNITY_EDITOR
        Debug.Log("Leave Game - Enable Lobby Manager!!!");
#endif
    }

    public async void CreateLobby(string roomName, int maxPlayer, bool isPrivate, string duration, string map = null, string map_weather = null)
    {
        Player player = GetPlayer();

        // Support default map and map weather
        string _map = map == null ? "The Town" : map;
        string _map_weather = map_weather == null ? "Sunny" : map_weather;

        // Options when create lobby, make this lobby private and add the player that created this lobby
        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
        {
            IsPrivate = isPrivate,
            Player = player,

            //! Add password control
            // Data for creating lobby, including: duration(minutes), map 
            Data = new Dictionary<string, DataObject> {
                    { KEY_DURATION, new DataObject(DataObject.VisibilityOptions.Public, duration) },
                    { KEY_MAP, new DataObject(DataObject.VisibilityOptions.Public, _map) },
                    { KEY_MAP_WEATHER, new DataObject(DataObject.VisibilityOptions.Public, _map_weather) }
                }
        };

        //! Check create lobby password
        // Create lobby async
        _hostLobby = await LobbyService.Instance.CreateLobbyAsync(roomName, maxPlayer, createLobbyOptions);
        _joinedLobby = _hostLobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = _hostLobby });

#if UNITY_EDITOR
        Debug.Log($"Lobby created. Lobby name: {_hostLobby.Name}; Max player: {_hostLobby.MaxPlayers}; Lobby code: {_hostLobby.LobbyCode}");
        PrintPlayers(_hostLobby);
#endif
    }

    public async void RefreshLobbyList()
    {
        try
        {
            // Get list of lobbies from query UGS Lobby
            // Query can have filter (optional game feature)
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = queryResponse.Results });

#if UNITY_EDITOR
            foreach (var result in queryResponse.Results)
            {
                Debug.Log($"Lobby name: {result.Name} Max player: {result.MaxPlayers}");
            }
#endif
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }


    #region Join Lobby
    //! Check use case: if code input is not correct
    public async void JoinLobbyByCode(string lobbyCode)
    {
        Player player = GetPlayer();

        //! Check join with password
        // Join a player to a lobby async using lobby code
        Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions
        {
            Player = player
        });
        _joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

#if UNITY_EDITOR
        PrintPlayers(_joinedLobby);
#endif

    }

    public async void JoinLobbyByID(Lobby lobby)
    {
        Player player = GetPlayer();

        _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
        {
            Player = player
        });

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = _joinedLobby });

#if UNITY_EDITOR
        PrintPlayers(_joinedLobby);
#endif
    }
    #endregion


    #region Player
    private Player GetPlayer()
    {
        // Return a new Player with data of player name, character and side weapon
        return new Player(_playerID, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, _playerName)},
            { KEY_PLAYER_CHARACTER, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerCharacter.None.ToString()) },
            { KEY_SIDE_WEAPON, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, SideWeapon.Knife.ToString()) }
        });
    }

    public string GetPlayerName()
    {
        return _playerName;
    }

    //! Put update player name on Game Settings
    public async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            _playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, _playerID, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject> {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, _playerName) }
                }
            });
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void UpdatePlayerCharacter(PlayerCharacter playerCharacter)
    {
        if (_joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions {
                    Data = new Dictionary<string, PlayerDataObject>() {
                    { KEY_PLAYER_CHARACTER, new PlayerDataObject( PlayerDataObject.VisibilityOptions.Public, playerCharacter.ToString()) }
                    }
                };

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, _playerID, options);
                _joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = _joinedLobby });
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }
        }
    }

    public async void UpdatePlayerSideWeapon(SideWeapon sideWeapon)
    {
        if (_joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions
                {
                    Data = new Dictionary<string, PlayerDataObject>() {
                    { KEY_SIDE_WEAPON, new PlayerDataObject( PlayerDataObject.VisibilityOptions.Public, sideWeapon.ToString()) }
                    }
                };

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, _playerID, options);
                _joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = _joinedLobby });
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }
        }
    }

    public async void KickPlayer(string playerID)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerID);
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }
        }
    }

#if UNITY_EDITOR
    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }
#endif
    #endregion

    // Allow player to leave lobby
    public async void LeaveLobby()
    {
        if (_joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, _playerID);
                _joinedLobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }
        }
    }

    // Migrate the lobby host to another player. Just keep it in case of need
    //private async void MigrateLobbyHost(string newHostID)
    //{
    //    try
    //    {
    //        _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions
    //        {
    //            HostId = newHostID
    //        });
    //        _joinedLobby = _hostLobby;
    //    }
    //    catch (LobbyServiceException ex)
    //    {
    //        Debug.Log(ex);
    //    }
    //}

    //! Add kick player out of lobby (optional)
}
