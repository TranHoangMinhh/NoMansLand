using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPositions : MonoBehaviour
{
    
    public static PlayerSpawnPositions Instance {  get; private set; }

    private List<Vector3> _spawnPositionList;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _spawnPositionList = new List<Vector3>();

        LoadSpawnPositions();
    }

    public List<Vector3> GetPlayerSpawnPositions()
    {
        return _spawnPositionList;
    }

    private void LoadSpawnPositions()
    {
        foreach (Transform child in transform)
        {
            _spawnPositionList.Add(child.position);
        }
    }

}
