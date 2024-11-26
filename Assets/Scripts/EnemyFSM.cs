using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack
    }
    
    private EnemyState m_State;
    
    private float findDistance = 100f;
    
    private GameObject _player;
    private Player _sPlayer;

    private float attackDistance = 5f;
    
    private float moveSpeed = 5f;
    
    private float currentTime = 0f;
    
    private float attackDelay = 2f;

    private GameObject target;
    
    private Animator anim;
    
    private NavMeshAgent smith;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _sPlayer = _player.GetComponent<Player>();
        target = _player;
        m_State = EnemyState.Idle;
        
        anim = transform.GetComponentInChildren<Animator>();
        smith = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        smith.isStopped = false;
        smith.ResetPath();
            
        smith.stoppingDistance = attackDistance;
        smith.destination = _player.transform.position;

        Collider[] moveColliders = Physics.OverlapSphere(transform.position, 20f);
        if (moveColliders.Length == 0)
        {
            target = _player;
        }
        for (int i = 0; i < moveColliders.Length; i++)
        {
            if (moveColliders[i].gameObject.tag == "Survivor")
            {
                target = moveColliders[i].gameObject;
            }
        }
        if (Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
        {
            m_State = EnemyState.Attack;
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                transform.LookAt(_player.transform.position);
                currentTime = 0;
                anim.SetTrigger("StartAttack");
                _sPlayer.hp -= 5;
            }
        }
        else
        {
            m_State = EnemyState.Move;
            currentTime = 0;

            anim.SetTrigger("AttackToMove");
        }
    }
    
}
