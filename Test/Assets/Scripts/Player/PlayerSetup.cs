using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Transform characterSkins;
    private List<Transform> characterSkinsList;
    PlayerData playerData; 

    private void Awake()
    {
        characterSkinsList = new List<Transform>();
        foreach (Transform child in characterSkins)
        {
            if (child.name != "root")
                characterSkinsList.Add(child);
            Debug.Log($"Number of objects: {characterSkinsList.Count}");
        }
        playerData = NMLGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);

    }

    private void Start()
    {
        SetPlayer(playerData.skinId);
    }

    private void SetPlayer(int skinId)
    {
        Debug.Log($"Client ID - {OwnerClientId} - Skin ID: {skinId}");
        if(skinId != -1)
        {
            DisableAllCharacterObject();
            characterSkinsList[skinId].gameObject.SetActive(true);
        }
    }

    private void DisableAllCharacterObject()
    {
        foreach (Transform child in characterSkins)
        {
            if (child.name != "root")
                child.gameObject.SetActive(false);
        }
    }

}
