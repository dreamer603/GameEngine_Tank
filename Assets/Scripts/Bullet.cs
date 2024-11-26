using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject boomEffect;
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * 50f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log(other.gameObject.name);
        boomEffect.transform.position = transform.position;
        Instantiate(boomEffect);
        Collider[] cols = Physics.OverlapSphere(transform.position, 2.25f);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.CompareTag("Zombie") || cols[i].gameObject.CompareTag("CanDestroy"))
            {
                Destroy(cols[i].gameObject);
            }

            if (cols[i].gameObject.CompareTag("Survivor"))
            {
                Destroy(cols[i].gameObject);
                LoadSceneManager.Instance.GameOver();
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 2.25f);
    }
}
