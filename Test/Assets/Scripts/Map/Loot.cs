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
    [SerializeField] private int damage;

    public Loot(GameObject lootWeapon, int dropChance, int damage)
    {
        this.lootWeapon = lootWeapon;
        this.dropChance = dropChance;
        this.damage = damage;
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

    public int GetWeaponDamage()
    {
        return this.damage;
    }
}
