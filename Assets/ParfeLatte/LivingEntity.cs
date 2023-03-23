using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float Maxhealth;//�ִ�ü��
    public float health;//����ü��
    public float damage;//���ݽ� �ִ� ������
    public float MoveSpeed;//�̵��ӵ�

    public bool isDead;//������� 
    
    protected virtual void OnEnabled()
    {
        isDead = false;//����ִ� ���·� ����    
    }

    protected virtual void Damaged(float Damage)
    {
        health -= Damage;//��������ŭ ü�±���
        
        if(health <= 0 && !isDead)//ü���� 0�����̰� ���ó�� �ȵ����� 
        {
            Die();//����̺�Ʈ
        }
    }//���ݹ޾����� ü�±�� ������� Ȯ�� ó��

    public virtual void Die()
    {
        //�׾����� ������ �ִϸ��̼� ���(����ϸ鼭)
        isDead = true;//���ó��
    }

    protected virtual void SetStatus(float MaxHP, float Dmg, float Spd)
    {
        Maxhealth = MaxHP;
        damage = Dmg;
        MoveSpeed = Spd;
    }//�ִ�ü�°� ������, �̵��ӵ� ��������
}