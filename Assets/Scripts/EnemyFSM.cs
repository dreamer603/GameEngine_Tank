using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }
    
    private EnemyState m_State;
    
    public float findDistance = 8f;
    
    private Transform player;
    

    public float attackDistance = 2f;
    
    public float moveSpeed = 5f;
    
    private CharacterController cc;
    
    private float currentTime = 0;
    
    private float attackDelay = 2f;
    public int attackPower = 3;
    

    private Vector3 originPos;
    private Quaternion originRot;
    
    public float moveDistance = 20f;
    public int hp = 30;
    

    public int maxHp = 30;
    

    public Slider hpSlider;
    

    private Animator anim;
    
    private NavMeshAgent smith;
    
    void Start()
    {
        m_State = EnemyState.Idle;
        
        cc = GetComponent<CharacterController>();
        
        originPos = transform.position;
        originRot = transform.rotation;
        
        anim = transform.GetComponentInChildren<Animator>();
        smith = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        hpSlider.value = (float)hp / (float)maxHp;
        
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
            case EnemyState.Damaged:
                // Damaged();
                break;
            case EnemyState.Die:
                // Die();
                break;
        }
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
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
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            smith.isStopped = true;
            smith.ResetPath();
            
            smith.stoppingDistance = attackDistance;
            smith.destination = player.position;
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
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                currentTime = 0;
                
                anim.SetTrigger("StartAttack");
            }
        }
        else=

        {
            m_State = EnemyState.Move;
            currentTime = 0;

            anim.SetTrigger("AttackToMove");
        }
    }
    
    // 플레이어의 스크립트의 데미지 처리 함수를 실행하기
    public void AttackAction()
    {
        //player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        // 만일 초기 위치에서의 거리가 0.1f 이상이라면 초기 위치쪽으로 이동한다.
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            smith.destination = originPos;

            smith.stoppingDistance = 0;
        }
        else 
        {
            smith.isStopped = true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            
            hp = maxHp;
            m_State = EnemyState.Idle;
  
            anim.SetTrigger("MoveToIdle");
        }
    }
    
    public void HitEnemy(int hitPower)
    {
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }
        
        hp -= hitPower;
        smith.isStopped = true;
        smith.ResetPath();
        
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            anim.SetTrigger("Damaged");
            Damaged();
        }
        else 
        {
            m_State = EnemyState.Die;
            anim.SetTrigger("Die");
            Die();
        }
    }
    
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1f);
        m_State = EnemyState.Move;
    }
    
    // 죽음 상태 메서드
    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
