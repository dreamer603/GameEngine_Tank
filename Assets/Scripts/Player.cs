using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject tankTurret;
    public GameObject tankGun;
    public GameObject bullet;
    public GameObject boomEffect;

    public Transform firePosition;

    private CharacterController _characterController;

    private float _moveSpeed = 4f;

    private float _rotateSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().Fire(50f);
        }
    }

    private void Move()
    {
        float dirZ = Input.GetAxis("Vertical");
        float dirX = Input.GetAxis("Horizontal");
        
        Vector3 rot = new Vector3(0, dirX, 0).normalized;
        
        
        if (dirZ == 1)
        {
            gameObject.transform.Rotate(rot * _rotateSpeed * Time.deltaTime);
            _characterController.Move(transform.forward * _moveSpeed * Time.deltaTime);
        }
        else if (dirZ == -1)
        {
            gameObject.transform.Rotate(rot * _rotateSpeed * Time.deltaTime);
            _characterController.Move(-transform.forward * _moveSpeed * Time.deltaTime);
        }

        if (!_characterController.isGrounded)
        {
            _characterController.Move(new Vector3(0, -9.8f, 0) * Time.deltaTime);
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
            if (transform.eulerAngles.x >= 0f)
            {
                Debug.Log("cancel1");
                return;
            }
            float rotGun = Input.GetAxis("Mouse ScrollWheel") * _rotateSpeed * Time.deltaTime;
            tankGun.transform.Rotate(new Vector3(rotGun, 0, 0));
        }
        else if (inputScroll < 0)
        {
            if (transform.eulerAngles.x <= 340f && transform.eulerAngles.x != 0f)
            {
                Debug.Log("cancel2");
                return;
            }
            float rotGun = Input.GetAxis("Mouse ScrollWheel") * _rotateSpeed * Time.deltaTime;
            tankGun.transform.Rotate(new Vector3(rotGun, 0, 0));
        }
    }
    
    
}
