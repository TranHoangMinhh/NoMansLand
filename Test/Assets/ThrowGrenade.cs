using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

public class ThrowGrenade : MonoBehaviour
{
    public GameObject smokeGrenadePrefab;
    public float throwForce = 10f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowSmokeGrenade();
        }
    }

    private void ThrowSmokeGrenade()
    {
        // Instantiate the smoke grenade prefab
        GameObject smokeGrenade = Instantiate(smokeGrenadePrefab, transform.position, Quaternion.identity);

        // Get the forward direction of the player's camera
        Vector3 throwDirection = mainCamera.transform.forward;

        // Apply the throw force to the smoke grenade in the throw direction
        Rigidbody rb = smokeGrenade.GetComponent<Rigidbody>();
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }
}

