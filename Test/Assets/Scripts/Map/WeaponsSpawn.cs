using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class WeaponsSpawn : NetworkBehaviour
{
    [SerializeField] private List<Loot> weapons;
    private List<GameObject> weaponPool;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InitializePool();

        if (IsServer)
        {
            InstantiateLootServerRpc(transform.position);
        }
        else
        {
            InstantiateLootClientRpc();
        }
    }

    public void InitializePool()
    {
        weaponPool = new List<GameObject>();

        foreach (Loot loot in weapons)
        {
            GameObject weapon = loot.GetWeapon();
            if (weapon != null)
            {
                weaponPool.Add(weapon);
            }
        }
    }

    private Loot GetDroppedWeapon()
    {
        int randomChance = Random.Range(1, 101);

        List<Loot> loots = new List<Loot>();

        foreach (Loot loot in weapons)
        {
            if (randomChance <= loot.GetDroppedChance())
            {
                loots.Add(loot);
            }
        }

        if (loots.Count > 0)
        {
            Loot droppedWeapon = loots[Random.Range(0, loots.Count)];
            return droppedWeapon;
        }

        Debug.Log("No Weapon Is Spawned!!!");
        return null;
    }

    [ServerRpc]
    public void InstantiateLootServerRpc(Vector3 spawnPosition)
    {
        Loot weapon = GetDroppedWeapon();
        if (weapon != null)
        {
            GameObject weaponPrefab = weapon.GetWeapon();
            if (weaponPrefab != null)
            {
                GameObject spawnedWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity);
                NetworkObject networkObject = spawnedWeapon.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    networkObject.Spawn();
                }
            }
        }
    }

    [ClientRpc]
    public void InstantiateLootClientRpc()
    {
        Loot weapon = GetDroppedWeapon();
        if (weapon != null)
        {
            GameObject weaponPrefab = weapon.GetWeapon();
            if (weaponPrefab != null && weaponPool.Contains(weaponPrefab))
            {
                GameObject spawnedWeapon = Instantiate(weaponPrefab);
            }
        }
    }
}
