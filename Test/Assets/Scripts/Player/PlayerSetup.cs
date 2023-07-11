using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private List<GameObject> spriteList;
    private void Start()
    {
    }

    private void SetPlayer()
    {
        SetPlayerServerRpc();
    }

    [ServerRpc]
    private void SetPlayerServerRpc()
    {
    }

}
