using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprites : MonoBehaviour
{
    
    public static CharacterSprites Instance {  get; private set; }

    [SerializeField] private List<Sprite> charactersList;

    private Dictionary<LobbyManager.PlayerCharacter, Sprite> _characterDictionary;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _characterDictionary = new Dictionary<LobbyManager.PlayerCharacter, Sprite>();
        LoadCharacterList();
    }

    private void LoadCharacterList()
    {
        int index = 0;

        foreach (LobbyManager.PlayerCharacter character in Enum.GetValues(typeof(LobbyManager.PlayerCharacter)))
        {
            _characterDictionary.Add(character, charactersList[index]);
            index++;
        }
    }   

    public Sprite GetCharacterSprite(LobbyManager.PlayerCharacter character)
    {
        return _characterDictionary[character];
    }

}
