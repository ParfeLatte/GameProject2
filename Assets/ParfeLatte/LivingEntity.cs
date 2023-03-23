using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float Maxhealth;//최대체력
    public float health;//현재체력
    public float damage;//공격시 주는 데미지
    public float MoveSpeed;//이동속도

    public bool isDead;//생명상태 
    
    protected virtual void OnEnabled()
    {
        isDead = false;//살아있는 상태로 시작    
    }

    protected virtual void Damaged(float Damage)
    {
        health -= Damage;//데미지만큼 체력깎음
        
        if(health <= 0 && !isDead)//체력이 0이하이고 사망처리 안됐으면 
        {
            Die();//사망이벤트
        }
    }//공격받았을시 체력깎고 사망상태 확인 처리

    public virtual void Die()
    {
        //죽었을때 실행할 애니메이션 등록(상속하면서)
        isDead = true;//사망처리
    }

    protected virtual void SetStatus(float MaxHP, float Dmg, float Spd)
    {
        Maxhealth = MaxHP;
        damage = Dmg;
        MoveSpeed = Spd;
    }//최대체력과 데미지, 이동속도 설정해줌
}