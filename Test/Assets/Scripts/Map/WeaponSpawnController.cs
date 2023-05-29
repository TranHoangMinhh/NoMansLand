using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<WeaponsSpawn>().InstantiateLoot(transform.position);
    }

}
