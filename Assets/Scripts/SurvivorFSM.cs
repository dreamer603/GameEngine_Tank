using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class SurvivorFSM : MonoBehaviour
{
    enum SurvivorState
    {
        Idle,
        Move
    }
    
    private SurvivorState m_State;
    
    private float findDistance = 10f;
    
    private GameObject _player;
    
    private float moveSpeed = 5f;
    
    private float currentTime = 0;

    private Vector3 originPos;
    
    public float moveDistance = 20f;

    private NavMeshAgent smith;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        m_State = SurvivorState.Idle;
        
        originPos = transform.position;

        smith = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (m_State)
        {
            case SurvivorState.Idle:
                Idle();
                break;
            case SurvivorState.Move:
                Move();
                break;
        }
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < findDistance)
        {
            m_State = SurvivorState.Move;
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = SurvivorState.Idle;
        }
        else
        {
            smith.isStopped = false;
            smith.ResetPath();
            
            smith.destination = _player.transform.position;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.Score += 100;
            Destroy(gameObject);
        }
    }
}
