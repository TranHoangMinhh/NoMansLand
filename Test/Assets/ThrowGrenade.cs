using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

public class ThrowGrenade : MonoBehaviour
{
    public GameObject smokeGrenadePrefab;
    public GameObject explosionGrenadePrefab;
    public float throwForce = 10f;
    public int grenadeDamage = 50;
    public float explosionRadius = 10f;

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            ThrowExplosionGrenade();
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

    private void ThrowExplosionGrenade()
    {
        // Instantiate the explosion grenade prefab
        GameObject explosionGrenade = Instantiate(explosionGrenadePrefab, transform.position, Quaternion.identity);

        // Get the forward direction of the player's camera
        Vector3 throwDirection = mainCamera.transform.forward;

        // Apply the throw force to the explosion grenade in the throw direction
        Rigidbody rb = explosionGrenade.GetComponent<Rigidbody>();
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }
}
