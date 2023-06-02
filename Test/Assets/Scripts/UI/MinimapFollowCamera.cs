using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollowCamera : MonoBehaviour
{
    [SerializeField] private MinimapController controller;
    [SerializeField] private float cameraHeight;

    private void Awake()
    {
        controller = GetComponentInParent<MinimapController>();
        cameraHeight = transform.position.y;
    }

    private void Update()
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
