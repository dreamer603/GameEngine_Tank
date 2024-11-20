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

    public void Fire(float fireAngle, float fireVelocity)
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        float theta = fireAngle * Mathf.Deg2Rad;

        float velocityX = fireVelocity * Mathf.Cos(theta);
        float velocityY = fireVelocity * Mathf.Sin(theta);

        Vector3 force = new Vector3(velocityX, velocityY, 0);
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(this);
    }
}
