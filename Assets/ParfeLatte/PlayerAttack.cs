using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Player player;
    public Vector3 curPos;

    private float Damage;//���ϴ� ������
    private bool isAttack;//�����߳�


    // Start is called before the first frame update
    void Start()
    {
        curPos = transform.localPosition;
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.h == 1) {
            this.transform.localPosition=new Vector3(curPos.x, curPos.y, curPos.z);
        }
        if(player.h == -1)
        {
            this.transform.localPosition = new Vector3(curPos.x * -1, curPos.y, curPos.z);
        }
    }

    public void GetAttack(float damage)
    {
        Damage = damage;
        isAttack = true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("�������� �ֽ��ϴ�");
        if (col.tag == "enemy")
        {
            Debug.Log("���Ͱ� �������� �ֽ��ϴ�");
            if (isAttack)
            {
                Monster enemy = col.GetComponent<Monster>();
                enemy.damaged(Damage);
                isAttack = false;
            }
            //����ó��!
        }
    }
}
