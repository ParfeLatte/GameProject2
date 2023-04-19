using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    protected float MaxHealth;//�ִ�ü��
    public float Health;//����ü��
    protected float damage;//���ݽ� ���ϴ� ������
    protected float MaxSpeed;//�̵��ӵ�

    public bool isDead;//ĳ���Ͱ� ����ߴ��� Ȯ��


    public virtual void SetStatus(float HP, float Damage, float Speed)
    {
        MaxHealth = HP;
        damage = Damage;
        MaxSpeed = Speed;
    }//�⺻ ���� ����

    public virtual void damaged(float damage)
    {
        Health -= damage;//ü�¿��� ��������ŭ ����
        //���
        //����

        if(Health <= 0 && !isDead)
        {
            Die();
        }//ü���� 0�� �ǰų� 0�Ʒ��� �������� ���� �ʾҴٸ�
        //�ǰݽ� ������ ����
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (isDead)
        {
            // �̹� ����� ��� ü���� ȸ���� �� ����
            return;
        }

        // ü�� �߰�
        Health += newHealth;//ü�� ȸ��
        if(Health >= MaxHealth)
        {
            Health = MaxHealth;
        }//�ִ� ü�� �̻����� ȸ���� �� ����.
    }

    public virtual void Die()
    {
        isDead = true;
        //���
        //����
        //����ÿ� ������ �̺�Ʈ
    }
}
