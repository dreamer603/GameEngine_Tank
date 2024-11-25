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
    
    public float findDistance = 40f;
    
    private GameObject _player;
    private Player _sPlayer;

    private float attackDistance = 5f;
    
    private float moveSpeed = 5f;
    
    private CharacterController cc;
    
    private float currentTime = 0;
    
    private float attackDelay = 2f;
    public int attackPower = 3;
    

    private Vector3 originPos;
    private Quaternion originRot;
    
    public float moveDistance = 20f;

    private Animator anim;
    
    private NavMeshAgent smith;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _sPlayer = _player.GetComponent<Player>(); 
        m_State = EnemyState.Idle;
        
        cc = GetComponent<CharacterController>();
        
        originPos = transform.position;
        originRot = transform.rotation;
        
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
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Idle;
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) > attackDistance)
        {
            smith.isStopped = false;
            smith.ResetPath();
            
            smith.stoppingDistance = attackDistance;
            smith.destination = _player.transform.position;
        }
        else 
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
