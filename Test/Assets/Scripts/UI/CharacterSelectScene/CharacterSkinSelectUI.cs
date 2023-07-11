using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkinSelectUI : MonoBehaviour
{
    [SerializeField] private int skinId;
    [SerializeField] private GameObject selectedGameObject;


    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => {
            NMLGameMultiplayer.Instance.ChangePlayerSkin(skinId);
        });
    }

    private void Start() {
        NMLGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += NMLGameMultiplayer_OnPlayerDataNetworkListChanged;
        UpdateIsSelected();
    }

    private void NMLGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e) {
        UpdateIsSelected();
    }

    private void UpdateIsSelected() {
        Debug.Log($"Client id {NMLGameMultiplayer.Instance.GetPlayerData().clientId} - Skin id: {NMLGameMultiplayer.Instance.GetPlayerData().skinId}");
        if (NMLGameMultiplayer.Instance.GetPlayerData().skinId == skinId) {
            selectedGameObject.SetActive(true);
        } else{
            selectedGameObject.SetActive(false);
        }
    }

    private void OnDestroy() {
        NMLGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= NMLGameMultiplayer_OnPlayerDataNetworkListChanged;
    }
}
