using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletProjectile : NetworkBehaviour
{
    private Rigidbody bulletRigidBody;

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
        Destroy(gameObject);
    }
}
