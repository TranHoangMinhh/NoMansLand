using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MinimapFollowCamera : NetworkBehaviour
{
    [SerializeField] private MinimapController controller;
    [SerializeField] private float cameraHeight;
    [SerializeField] private GameObject minimap;

    private void Awake()
    {
        controller = GetComponentInParent<MinimapController>();
        cameraHeight = transform.position.y;
    }

    private void Update()
    {
        if (IsOwner)
        {
            Vector3 targetPosition = controller.targetToFollow.transform.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y + cameraHeight, targetPosition.z);

            if (controller.rotateWithTheTarget)
            {
                Quaternion targetRotation = controller.targetToFollow.transform.rotation;
                transform.rotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, 0);
            }
        }
    }
}
