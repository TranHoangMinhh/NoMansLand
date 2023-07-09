using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletProjectile : NetworkBehaviour
{
    private Rigidbody bulletRigidBody;
    [SerializeField] private Transform bloodVFX;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        float speed = 40f;
        bulletRigidBody.velocity = transform.forward * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletProjectile>() != null)
        {
            
        }
        Destroy(gameObject);
    }
    [ServerRpc]
    private void bloodEffectServerRpc()
    {
        Transform bloodEffect= Instantiate(bloodVFX, transform.position, Quaternion.identity);
        bloodEffect.GetComponent<NetworkObject>();
    }
}
