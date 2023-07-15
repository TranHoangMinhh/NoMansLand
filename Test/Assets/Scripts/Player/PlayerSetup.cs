using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Transform characterSkins;
    private List<Transform> characterSkinsList;

    private void Awake()
    {
        characterSkinsList = new List<Transform>();
        foreach (Transform child in characterSkins)
        {
            if (child.name != "root")
                characterSkinsList.Add(child);
            Debug.Log($"Number of objects: {characterSkinsList.Count}");
        }

    }

    private void Start()
    {
        SetPlayer(NMLGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId).skinId);
    }

    //NMLGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId).diedNumber

    private void SetPlayer(int skinId)
    {
        Debug.Log($"Client ID - {OwnerClientId} - Skin ID: {NMLGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId).skinId}");
        if(skinId != -1)
        {
            DisableAllCharacterObject();
            characterSkinsList[NMLGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId).skinId].gameObject.SetActive(true);
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
