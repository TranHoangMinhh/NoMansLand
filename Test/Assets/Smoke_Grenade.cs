using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke_Grenade : MonoBehaviour
{
    public GameObject smoke;
    public float duration;
    MeshRenderer renderer;


    float countdown;
    bool hasExploded = false;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();
        Invoke("ActivateSmoke",1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void ActivateSmoke()
    {
        GameObject smokeInstace = Instantiate(smoke, transform.position, transform.rotation);
        renderer.enabled = false;
        // Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
    }

}


