using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //public GameObject small;//���� 15�� �ݶ��̴� ����
    //public GameObject Big;//���� 30�� �ݶ��̴� ����
    public GameObject Player;//�Ÿ�������

    public int SleepState;//���� ������� 0:��������, 1:�߰�����, 2:��������, 3:���!!!
    public float moveTimeOne;//�÷��̾ �󸶳� �ɾ����� Ȯ��
    public float moveTimeTwo;//�÷��̾ �󸶳� �ɾ����� Ȯ��
    public float CheckTime;//���� �ȿ� ���� �ð�
    public float Dist;//���Ϳ� �÷��̾� ���� �Ÿ�
    
    public Vector3 lastPlayerPosition;//

    public Player player;//�÷��̾� �ڵ�

    public bool isPlayerDash;//�÷��̾ �뽬�ߴ��� Ȯ��
    public bool isPlayerStop;//�÷��̾ ����°�?

    void Awake()
    {
        SleepState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//�÷��̾�� ���� ���� �Ÿ�
        
        if (Dist <= 30)
        {
            if(SleepState == 0)
            {
                moveTimeOne += Time.deltaTime;
            }//���� �����϶� �˻��ϴ� �ð�
            else if(SleepState  == 1 && Dist <= 15)
            {
                moveTimeTwo += Time.deltaTime;
            }//�߰� �����϶� �˻��ϴ� �ð�

            if (!player.isMove)
            {
                StartCoroutine("CheckStop");
            }//�������� ����˻�
            else
            {
                isPlayerStop = false;
            }//������ �ʾ���
            AreaCheck(SleepState);//����˻�!
        }


       
    }

    IEnumerator CheckStop()
    {
        lastPlayerPosition = Player.transform.position;

        yield return new WaitForSeconds(0.1f);
        Debug.Log("�˻��մϴ�.");
        if (lastPlayerPosition == Player.transform.position)
        {
            Debug.Log("������ϴ�.");
            moveTimeOne = 0;
            moveTimeTwo = 0;
            isPlayerStop = true;
        }//�ح����Ƿ� �˻��ϴ� �ð��� �ʱ�ȭ
        else
        {
            Debug.Log("��� �����̴� ���Դϴ�.");
        }//��� �������� Ȯ��
    }//��������� Ȯ���� 0.4�ʰ� �������� �ʾƾ� ����ɷ� �ӽ����� 
    
    public void AreaCheck(int state)
    {
        switch (state)
        {
            case 0:
                if (Dist <= 30)
                {
                    if (moveTimeOne >= 2.0)
                    {
                        SleepState = 1;
                        moveTimeOne = 0;
                    }
                }//�Ÿ��� 30�ȿ� ������ üũ(�����������)
                break;
            case 1:
                if (SleepState == 1 && Dist <= 15)
                {
                    if (moveTimeTwo >= 0.8)
                    {
                        SleepState = 2;
                    }
                    else if(isPlayerStop)
                    {
                        SleepState = 0;
                    }
                }//�߰����� �����̰� 15�ȿ��� 0.8�ʰ� ��������?
                break;
            case 2:
                if (SleepState == 2)
                {
                    if (player.GetMoveCheck())
                    {
                        SleepState = 3;
                    }
                    else
                    {
                        SleepState = 1;
                    }
                }//���� ���� ���� �� ���¿� �÷��̾ ������ �ٷ� ���
                break;
        }
        CheckPlayerDash();
    }
    public void CheckPlayerDash()
    {
        if (player.GetDashCheck() == true)
        {
            SleepState = 3;
        }
    }//�÷��̾ �뽬�ߴ��� üũ��
}
/*�÷��̾�� �Ÿ��� 30���ϰ� �ȴ�  -> �˻���� -> 2�ʰ� ������ �߰�����
				          -> �뽬�ϸ� �ٷ� ���
�߰����鿡�� 15�Ÿ����� ���԰� �ȿ��� 0.8�ʰ� �ɾ��°�? ->  ���� ���� -> �������ڸ��� ���
->  �ƴϸ� �ٽ�  ���� ��������*/
//��->��->��->��(����)