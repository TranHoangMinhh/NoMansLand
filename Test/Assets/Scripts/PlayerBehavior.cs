using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerTakeDmg(20);
            //Debug.Log(GameManager.gameManager._playerHealth.Health);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerHeal(10);
            //Debug.Log(GameManager.gameManager._playerHealth.Health);
        }
    }

    private void PlayerTakeDmg(int dmg)
    {
        //GameManager.gameManager._playerHealth.DmgUnit(dmg);
        Healthbar.Instance.UpdateHealthBar(20);
    }

    private void PlayerHeal(int heal)
    {
        GameManager.gameManager._playerHealth.HealUnit(heal);
        Healthbar.Instance.UpdateHealthBar(GameManager.gameManager._playerHealth.Health);
    }
}
