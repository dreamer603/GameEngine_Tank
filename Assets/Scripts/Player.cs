using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Objects")]
    public GameObject tankTurret;
    public GameObject tankGun;
    [Space]
    public GameObject bullet;
    public GameObject boomEffect;
    public GameObject dieEffect;
    
    [Header("Components")]
    public Transform firePosition;
    [Space]
    public WheelCollider[] wheelColliders;
    private Rigidbody _rigidbody;
    
    [Header("amounts")]
    private float _motorToque = 150f;
    private float _rotateSpeed = 10f;
    private float _recoilForce = 10f;
    public float hp = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        Fire();
        if (hp == 0)
        {
            Die();
        }
    }
    

    private void Move()
    {
        float dirZ = Input.GetAxis("Vertical");
        float dirX = Input.GetAxis("Horizontal");
        
        Vector3 rot = new Vector3(0, dirX, 0).normalized;

        if (dirZ == 0)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = 0f;
                wheelColliders[i].brakeTorque = _motorToque;
            }
        }
        else
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].brakeTorque = 0f;
                wheelColliders[i].motorTorque = dirZ * _motorToque;
            }

            gameObject.transform.Rotate(rot * _rotateSpeed * Time.deltaTime);
        }
    }
    
    private void Rotate()
    {
        float rotZ = Input.GetAxis("Mouse X");
        Vector3 rotTurret = new Vector3(0, 0, rotZ).normalized;
        tankTurret.transform.Rotate(rotTurret * _rotateSpeed * Time.deltaTime);
        Camera.main.transform.Rotate(new Vector3(0, rotZ, 0) * _rotateSpeed * Time.deltaTime);
        
        float inputScroll = Input.GetAxis("Mouse ScrollWheel");
        if (inputScroll > 0)
        {
            Debug.Log("1");
            if (tankGun.transform.rotation.x > 0.1f)
            {
                Debug.Log("cancel1");
                return;
            }
            float rotGun = Input.GetAxis("Mouse ScrollWheel") * _rotateSpeed * Time.deltaTime;
            tankGun.transform.Rotate(new Vector3(rotGun, 0, 0));
        }
        else if (inputScroll < 0)
        {
            if (tankGun.transform.rotation.x <= -0.8f && tankGun.transform.rotation.x != 0f)
            {
                Debug.Log("cancel2 :" + tankGun.transform.rotation.x);
                return;
            }
            Debug.Log("힝 속았지?");
            float rotGun = Input.GetAxis("Mouse ScrollWheel") * _rotateSpeed * Time.deltaTime;
            tankGun.transform.Rotate(new Vector3(rotGun, 0, 0));
        }
    }

    private void Fire()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        _rigidbody.AddForce(-firePosition.forward * _recoilForce, ForceMode.Impulse);
        boomEffect.transform.position = firePosition.position;
        Instantiate(boomEffect);
        Instantiate(bullet, firePosition.position, transform.rotation);
    }

    private void Die()
    {
        dieEffect.transform.position = transform.position;
        Instantiate(dieEffect);
        Destroy(gameObject);
    }
}
