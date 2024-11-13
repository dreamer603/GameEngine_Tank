using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject tankTurret;
    public GameObject tankGun;

    private CharacterController _characterController;

    private float _moveSpeed = 4f;

    private float _rotateSpeed = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirZ = Input.GetAxis("Vertical");
        float dirX = Input.GetAxis("Horizontal");
        
        Vector3 dir = new Vector3(0, 0, dirZ).normalized;
        Vector3 rot = new Vector3(0, dirX, 0).normalized;
        
        gameObject.transform.Rotate(rot * _rotateSpeed * Time.deltaTime);
        _characterController.Move(dir * _moveSpeed * Time.deltaTime);

        float rotZ = Input.GetAxis("Mouse X");
        Vector3 rotTurret = new Vector3(0, 0, rotZ).normalized;
        tankTurret.transform.Rotate(rotTurret * _rotateSpeed * Time.deltaTime);

        float rotGun = Math.Clamp(Input.GetAxis("Mouse ScrollWheel") * _rotateSpeed * Time.deltaTime,-20f, 0f);
        tankGun.transform.localRotation = Quaternion.Euler(rotGun, 0, 0);
    }
}
