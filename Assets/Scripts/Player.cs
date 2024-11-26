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
    
    private float _motorToque = 150f;
    private float _rotateSpeed = 50f;
    private float _recoilForce = 7f;
    public float maxHp = 100;
    public float hp = 100;
    private float _powerGauge = 1f;
    private bool _isCharging = false;
    
    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
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
            wheelColliders[0].steerAngle = dirX * 45;
            wheelColliders[6].steerAngle = dirX * 45;
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].brakeTorque = 0f;
                wheelColliders[i].motorTorque = dirZ * _motorToque;
            }
        }
    }
    
    private void Rotate()
    {
        float rotZ = Input.GetAxis("Mouse X");
        Vector3 rotTurret = new Vector3(0, 0, rotZ).normalized;
        tankTurret.transform.Rotate(rotTurret * _rotateSpeed * Time.deltaTime);
        
        float inputScroll = Input.GetAxis("Mouse ScrollWheel");
        if (inputScroll > 0)
        {
            Debug.Log("1");
            if (tankGun.transform.rotation.x >= -0.6f)
            {
                return;
            }
            float rotGun = Input.GetAxis("Mouse ScrollWheel") * _rotateSpeed * Time.deltaTime;
            tankGun.transform.Rotate(new Vector3(rotGun, 0, 0));
        }
        else if (inputScroll < 0)
        {
            if (tankGun.transform.rotation.x <= -0.8f && tankGun.transform.rotation.x != 0f)
            {
                return;
            }
            float rotGun = Input.GetAxis("Mouse ScrollWheel") * _rotateSpeed * Time.deltaTime;
            tankGun.transform.Rotate(new Vector3(rotGun, 0, 0));
        }
    }

    private void Fire()
    {
        if (!Input.GetMouseButtonDown(0) || _isCharging)
        {
            return;
        }
        
        _rigidbody.AddForce(-firePosition.forward * _recoilForce, ForceMode.Impulse);
        boomEffect.transform.position = firePosition.position;
        Instantiate(boomEffect);
        bullet.transform.position = firePosition.position;
        bullet.transform.rotation = firePosition.rotation;
        Instantiate(bullet);
        StartCoroutine(ChargePowerGauge());
        StartCoroutine(UIManager.Instance.ChargePowerGaugeUI());
    }

    private void Die()
    {
        dieEffect.transform.position = transform.position;
        Instantiate(dieEffect);
        Destroy(gameObject);
        GameManager.Instance.GameOver();
    }

    private IEnumerator ChargePowerGauge()
    {
        _isCharging = true;
        yield return new WaitForSeconds(1f);
        _isCharging = false;
    }
}
