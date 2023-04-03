using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public Monster monster;//����(������ ��ü)
    public Vector3 curPos;//���� ���ݹ��� ��ġ

    void Start()
    {
        curPos = transform.localPosition;//�θ� ������Ʈ�� �������� 
    }

    void Update()
    {
        if (monster.Dir == 1)
        {
            this.transform.localPosition = new Vector3(curPos.x, curPos.y, curPos.z);//���� ���� ��ġ ����(�÷��̾� ������)
        }
        if (monster.Dir == -1)
        {
            this.transform.localPosition = new Vector3(curPos.x * -1, curPos.y, curPos.z);//���� ���� ��ġ ����(�÷��̾� ����)
        }
    }
}
