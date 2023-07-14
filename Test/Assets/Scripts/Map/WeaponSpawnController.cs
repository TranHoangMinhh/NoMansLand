using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponSpawnController : NetworkBehaviour
{
    private WeaponsSpawn weaponsSpawn;

    private void Start()
    {
        InitializePool();

        if (IsServer)
        {
            weaponsSpawn.InstantiateLootServerRpc(transform.position);
        }
        else
        {
            weaponsSpawn.InstantiateLootClientRpc();
        }
    }

    private void InitializePool()
    {
        weaponsSpawn = GetComponent<WeaponsSpawn>();
        weaponsSpawn.InitializePool();
    }
}
