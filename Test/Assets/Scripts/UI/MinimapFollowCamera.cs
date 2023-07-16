using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MinimapFollowCamera : NetworkBehaviour
{
    [SerializeField] private MinimapController controller;
    //[SerializeField] private GameObject minimap;

    private float _cameraHeight;

    private void Awake()
    {
        controller = GetComponentInParent<MinimapController>();
        _cameraHeight = transform.position.y;
    }

    private void Update()
    {
        if (IsOwner)
        {
            Vector3 targetPosition = controller.targetToFollow.transform.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y + _cameraHeight, targetPosition.z);

            //if (controller.rotateWithTheTarget)
            //{
            //    Quaternion targetRotation = controller.targetToFollow.transform.rotation;
            //    transform.rotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, 0);
            //}
        }
    }
}
