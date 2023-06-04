using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager {  get; private set; }

    public UnitHealth _playerHealth = new UnitHealth(100, 100);

    private void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
        }
    }

    static void submitNewPosition()
    {
        
    }
}
