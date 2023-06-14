using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Player player;//�÷��̾�(������ ��ü)
    public Vector3 curPos;//���� ���ݹ����� ��ġ

    private float Damage;//���ϴ� ������
    private bool isAttack;//�����߳�

    public List<Collider2D> TargetList = new List<Collider2D>();//���� ������ ��Ƴ��� ����Ʈ

    // Start is called before the first frame update
    void Start()
    {
        curPos = transform.localPosition;//�θ� ������Ʈ�� �������� ��
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if(player.Dir == 1) {
            this.transform.localPosition = new Vector3(curPos.x, curPos.y, curPos.z);//���� ���� ��ġ ����(�÷��̾� ������)
        }
        if (player.Dir == -1)
        {
            this.transform.localPosition = new Vector3(curPos.x * -1, curPos.y, curPos.z);//���� ���� ��ġ ����(�÷��̾� ����)
        }
    }

    public void GetAttack(float damage)
    {
        Damage = damage;//������ �������� ����
        if (!isAttack)//�̹� �������϶��� ����� ����ó��
        {
            isAttack = true;//�������̴�!
            Invoke("AttackMonster", 0.2f);//���� ����(������ �ִ� ó��)
        }
    }

    private void AttackMonster()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            Monster enemy = TargetList[i].GetComponent<Monster>();//��󿡰Լ� ���� ��ũ��Ʈ��  �޾ƿ�
            enemy.damaged(Damage);//���ݹ����� �θ��� �Լ�
        }
        isAttack = false;
    }//���� ������� �����ִ� ��� ���Ϳ��� �������� ��

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.tag == "enemy" && !TargetList.Contains(col))
        {
            if (TargetList.Count < 5)
            {
                TargetList.Add(col);//���Ͱ� ���ݹ��� ���� ������ ���ݴ�� �߰�
            }
        }
        else
        {

        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (TargetList.Contains(col))
        {
            TargetList.Remove(col);//������ ����� ����Ʈ�� �ִ� ���͸� ������(���ݴ��X)
        }
        else
        {

        }
    }
}


//private void OnTriggerStay2D(Collider2D col)
//{
//    //if (col.tag == "enemy")
//    //{
//    //    if (isAttack)
//    //    {
//    //        Monster enemy = col.GetComponent<Monster>();
//    //        enemy.damaged(Damage);
//    //        isAttack = false;
//    //    }
//    //    //����ó��!
//    //}
//}
