using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoRandomLoader : MonoBehaviour
{

    [SerializeField] private List<VideoClip> clipList;
    [SerializeField] private VideoPlayer player;

    private int _videoIndex;


    private void Start()
    {
        _videoIndex = LobbyManager.Instance.menuBackgroundIndex;
        player.clip = clipList[_videoIndex];
    }

}
