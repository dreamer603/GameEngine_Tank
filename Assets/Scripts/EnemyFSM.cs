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
        Return,
        Damaged,
        Die
    }
    
    // 에너미 상태 변수
    private EnemyState m_State;
    
    // 플레이어 발견 범위
    public float findDistance = 8f;
    
    // 플레이어 트랜스폼
    private Transform player;
    
    // 공격 가능 범위
    public float attackDistance = 2f;
    
    // 이동 속도
    public float moveSpeed = 5f;
    
    // 캐릭터 콘트롤러 컴포넌트
    private CharacterController cc;
    
    // 누적 시간
    private float currentTime = 0;
    
    // 공격 딜레이 시간
    private float attackDelay = 2f;
    
    // 에너미의 공격력
    public int attackPower = 3;
    
    // 초기 위치 저장용 변수
    private Vector3 originPos;
    private Quaternion originRot;
    
    // 이동 가능 범위
    public float moveDistance = 20f;
    
    // 에너미 체력
    public int hp = 30;
    
    // 에너미 최대 체력
    public int maxHp = 30;
    
    // 에너미 hp Slider 변수
    public Slider hpSlider;
    
    // 애니메이터 변수
    private Animator anim;
    
    // 내비게이션 에이전트 변수
    private NavMeshAgent smith;
    
    void Start()
    {
        // 최초의 에너미 상태는 대기(Idle)로 한다
        m_State = EnemyState.Idle;
        
        // 플레이어 트랜스폼 컴포넌트 받아오기
        player = GameObject.Find("Player").transform;
        
        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        
        // 자신의 초기 위치 및 회전값 저장하기
        originPos = transform.position;
        originRot = transform.rotation;
        
        // 자식 오브젝트로부터 애니메이터 변수 받아오기
        anim = transform.GetComponentInChildren<Animator>();
        // 내비게이션 에이전트 컴포넌트 받아오기
        smith = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 현재 hp(%)를 hp 슬라이더의 value에 반영한다.
        hpSlider.value = (float)hp / (float)maxHp;
        
        // 현재 상태를 체크해 해당 상태별로 정해진 기능을 수행하게 하고 싶다.
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
            case EnemyState.Return:
                Return();
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
        // 만일 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환한다.
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            Debug.Log($"상태 전환: Idle -> Move");
            
            // 이동 애니메이션으로 전환하기
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        // 만일 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면...
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태를 복귀(Return)로 전환한다.
            m_State = EnemyState.Return;
            Debug.Log("상태 전환 : Move -> Return");
        }
        // 만일 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다.
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // // 이동 방향 설정
            // Vector3 dir = (player.position - transform.position).normalized;
            //
            // // 캐릭터 컨트롤러를 이용해 이동하기
            // cc.Move(dir * moveSpeed * Time.deltaTime);
            //
            // // 플레이어를 향해 방향을 전환한다.
            // transform.forward = dir;
            
            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
            smith.isStopped = true;
            smith.ResetPath();
            
            // 내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
            smith.stoppingDistance = attackDistance;
            // 내비게이션의 목적지를 플레이어의 위치로 설정한다.
            smith.destination = player.position;
        }
        else // 그렇지 않다면 현재 상태를 공격(Attack)으로 전환한다.
        {
            m_State = EnemyState.Attack;
            Debug.Log("상태 전환: Move -> Attack");
            // 누적 시간을 공격 딜레이 시간 만큼 미리 진행시켜 놓는다.
            currentTime = attackDelay;
            
            // 공격 대기 애니메이션 플레이
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        // 만일 플레이어가 공격 범위 이내에 있다면 플레이어를 공격한다.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격한다.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                Debug.Log("공격!");
                // [Ethan] 20241031 - HitEvent class로 해당 기능 이관
                // player.GetComponent<PlayerMove>().DamageAction(attackPower);
                currentTime = 0;
                
                // 공격 애니메이션 플레이
                anim.SetTrigger("StartAttack");
            }
        }
        else // 그렇지 않다면 현재 상태를 이동(Move)으로 전환한다 (재추격 실시)
        {
            m_State = EnemyState.Move;
            Debug.Log("상태 전환: Attack -> Move");
            currentTime = 0;
            
            // 이동 애니메이션 플레이
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
            // Vector3 dir = (originPos - transform.position).normalized;
            // cc.Move(dir * moveSpeed * Time.deltaTime);
            // // 방향을 복귀 지점으로 전환한다.
            // transform.forward = dir;
            
            // 내비게이션의 목적지를 초기 저장된 위치로 설정한다.
            smith.destination = originPos;
            
            // 내비게이션으로 접근하는 최소 거리를 '0'으로 설정한다.
            smith.stoppingDistance = 0;
        }
        else // 그렇지 않다면 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환한다.
        {
            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
            smith.isStopped = true;
            smith.ResetPath();
            
            // 위치 값과 회전 값을 초기 상태로 변환한다.
            transform.position = originPos;
            transform.rotation = originRot;
            
            // hp를 다시 회복한다.
            hp = maxHp;
            m_State = EnemyState.Idle;
            Debug.Log("상태 전환 : Return -> Idle");
            
            // 대기 애니메이션으로 전환하는 트랜지션을 호출한다.
            anim.SetTrigger("MoveToIdle");
        }
    }

    // 데미지 실행 메서드
    public void HitEnemy(int hitPower)
    {
        // 만일 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 메서드를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die ||
            m_State == EnemyState.Return)
        {
            return;
        }
        
        // 플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        hp -= hitPower;
        
        // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
        smith.isStopped = true;
        smith.ResetPath();
        
        // 에너미의 체력이 0보다 크면 피격 상태로 전환한다.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            Debug.Log("상태 전환 : Any State -> Damaged");
            // 피격 애니메이션을 플레이한다.
            anim.SetTrigger("Damaged");
            Damaged();
        }
        else // 그렇지 않다면 죽음 상태로 전환한다.
        {
            m_State = EnemyState.Die;
            Debug.Log("상태 전환 : Any State -> Die");
            // 죽음 애니메이션을 플레이한다.
            anim.SetTrigger("Die");
            Die();
        }
    }
    
    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamageProcess());
    }
    
    // 데미지 처리용 코루틴 메서드
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(1f);
        
        // 현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        Debug.Log("상태 전환 : Damaged -> Move");
    }
    
    // 죽음 상태 메서드
    void Die()
    {
        // 진행중인 피격 코루틴을 중지한다.
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 콘트롤러 컴포넌트를 비활성화한다.
        cc.enabled = false;
        
        // 2초 동안 기다린 후에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);
        Debug.Log("소멸");
        Destroy(gameObject);
    }
}
