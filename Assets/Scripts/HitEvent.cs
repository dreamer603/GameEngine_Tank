using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    // 에너미 스크립트 컴포넌트를 사용하기 위한 변수
    public EnemyFSM efsm;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    // 플레이어에게 데미지를 입히기 위한 이벤트 메서드
    public void PlayerHit()
    {
        efsm.AttackAction();
    }
}
