using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu]
public class Loot : ScriptableObject
{
    [SerializeField] private GameObject lootWeapon;
    [SerializeField] private string lootName;
    [SerializeField] private int dropChance;

    public Loot(GameObject lootWeapon, int dropChance)
    {
        this.lootWeapon = lootWeapon;
        this.dropChance = dropChance;
    }

    public int GetDroppedChance()
    {
        return this.dropChance;
    }

    public string GetLootName()
    {
        return this.lootName;
    }

    public GameObject GetWeapon()
    {
        return this.lootWeapon;
    }
}
