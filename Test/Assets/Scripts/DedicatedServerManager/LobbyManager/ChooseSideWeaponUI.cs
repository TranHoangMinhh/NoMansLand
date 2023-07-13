using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ChooseSideWeaponUI : MonoBehaviour
{

    [Header("Room Content")]
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject weaponVisual;
    [SerializeField] private Transform sideWeaponShowcase;
    [SerializeField] private TextMeshProUGUI sideWeaponName;

    [Header("Buttons")]
    [SerializeField] private List<Button> sideWeaponButtonList;
    [SerializeField] private Button lockInButton;


    private void Awake()
    {
        LoadButtonList();
        lockInButton.onClick.AddListener(StartGame);
    }

    private void Start()
    {
        playerVisual.SetActive(false);
        weaponVisual.SetActive(false);
    }

    private void LoadButtonList()
    {
        foreach (Button button in sideWeaponButtonList)
        {
            button.onClick.AddListener(() =>
            {
                LobbyManager.Instance.UpdatePlayerSideWeapon(button.GetComponent<ChooseObjectInfo>().GetSideWeapon());
                SetCharacterObjectActive(button.GetComponent<ChooseObjectInfo>().GetSideWeapon());
                sideWeaponName.text = button.GetComponent<ChooseObjectInfo>().GetSideWeapon().ToString();
                //int skinId = button.GetComponent<ChooseObjectInfo>().GetIndex();
                //NMLGameMultiplayer.Instance.ChangePlayerSkin(skinId);
                RemoveSelectedFX();

                if (!weaponVisual.activeSelf)
                {
                    weaponVisual.SetActive(true);
                }
            });
        }
    }

    private void RemoveSelectedFX()
    {
        foreach (Button button in sideWeaponButtonList)
        {
            if (button.GetComponent<ButtonBehavior>().HasButtonClicked())
            {
                button.GetComponent<ButtonBehavior>().RemoveClickedFX();
            }
        }
    }

    public void SetCharacterObjectActive(LobbyManager.SideWeapon sideWeapon)
    {
        string characterString = "Side_" + sideWeapon.ToString();
        foreach (Transform child in sideWeaponShowcase)
        {
            if (child.name == characterString)
            {
                DisableAllCharacterObject();
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void DisableAllCharacterObject()
    {
        foreach (Transform child in sideWeaponShowcase)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void StartGame()
    {
        //! Add functions to start the game
        //Debug.Log("Start Game!!!");
        NMLGameMultiplayer.Instance.PrintDataNetworkList();
        //Loader.LoadNetwork(Loader.Scene.GameScene);
        LoadingManager.Instance.LoadSceneNetwork(LoadingManager.Scenes.GameScene);
    }

}
