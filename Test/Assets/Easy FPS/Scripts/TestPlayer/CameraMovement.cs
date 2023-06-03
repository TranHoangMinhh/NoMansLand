using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class CameraMovement : NetworkBehaviour
{
    [SerializeField] CinemachineVirtualCamera vc;
    [SerializeField] AudioListener listener;
    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;
        }
        else
        {
            vc.Priority = 0;
        }
    }

}
