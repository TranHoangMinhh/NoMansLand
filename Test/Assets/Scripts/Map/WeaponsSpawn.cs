using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsSpawn : MonoBehaviour
{
    //[SerializeField] private GameObject weaponsSpawner;
    [SerializeField] private List<Loot> weapons;

    private Loot GetDroppedWeapon()
    {
        int randomChance = Random.Range(1, 101);

        List<Loot> loots = new List<Loot>();

        foreach (Loot loot in weapons)
        {
            if(randomChance <= loot.GetDroppedChance())
            {
                loots.Add(loot);
            }
        }

        if(loots.Count > 0)
        {
            Loot droppedWeapon = loots[Random.Range(0, loots.Count)];
            return droppedWeapon;
        }

        Debug.Log("No Weapon Is Spawned!!!");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot weapon = GetDroppedWeapon();
        if( weapon != null )
        {
            Instantiate(weapon.GetWeapon(), spawnPosition, Quaternion.identity);
        }
    }

    
}
