using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    protected float MaxHealth;//최대체력
    public float Health;//현재체력
    protected float damage;//공격시 가하는 데미지
    protected float MaxSpeed;//이동속도

    public bool isDead;//캐릭터가 사망했는지 확인


    public virtual void SetStatus(float HP, float Damage, float Speed)
    {
        MaxHealth = HP;
        damage = Damage;
        MaxSpeed = Speed;
    }//기본 스탯 설정

    public virtual void damaged(float damage)
    {
        Health -= damage;//체력에서 데미지만큼 깎음
        //모션
        //사운드

        if(Health <= 0 && !isDead)
        {
            Die();
        }//체력이 0이 되거나 0아래로 떨어졌고 죽지 않았다면
        //피격시 실행할 내용
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (isDead)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }

        // 체력 추가
        Health += newHealth;//체력 회복
        if(Health >= MaxHealth)
        {
            Health = MaxHealth;
        }//최대 체력 이상으로 회복할 수 없음.
    }

    public virtual void Die()
    {
        isDead = true;
        //모션
        //사운드
        //사망시에 진행할 이벤트
    }
}
