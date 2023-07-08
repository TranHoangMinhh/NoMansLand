using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundRandomLoader : MonoBehaviour
{

    [SerializeField] private List<Sprite> mapBackgroundList;
    [SerializeField] private Image background;
    [SerializeField] private bool generateRandomIndex;

    private int _mapIndex;


    private void Start()
    {
        if (generateRandomIndex)
        {
            _mapIndex = Random.Range(0, mapBackgroundList.Count);
            LobbyManager.Instance.menuBackgroundIndex = _mapIndex;
        }
        else
        {
            _mapIndex = LobbyManager.Instance.menuBackgroundIndex;
        }
        
        background.sprite = mapBackgroundList[_mapIndex];
    }


}
