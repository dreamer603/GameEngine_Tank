using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Fire(float fireVelocity)
    {
        Debug.Log("fire");
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * fireVelocity * Time.deltaTime, ForceMode.Impulse);
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(this);
    }
}
