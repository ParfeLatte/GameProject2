using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessPoint : MonoBehaviour
{
    public KeyCard keycard;
    public GameManager gameManager;
    public KeyCardEvent Event;

    public int accessLevel;//Űī�� ����

    public bool isEvent;//�̺�Ʈ�� ����������
    public bool isEnd;//�̺�Ʈ�� ���´���
    public bool CanAccess;//ī�忡 ���ٰ�������

    private bool isPlayer;//�÷��̾ �����ߴ���

    void Awake()
    {
        isEvent = false;
        isEnd = false;
        CanAccess = false;
        isPlayer = false;
        accessLevel = keycard.AccessLevel;
    }

    void Update()
    {
        
        if(isPlayer && Input.GetKeyDown(KeyCode.F)){
            switch (accessLevel)
            {
                case 4:
                    TargetCheck();
                    break;
                default:
                    CheckAccess();//�÷��̾ ���� ���� ������ U�� �������� ���ٽõ�
                    break;
            }
        }
    }

    public void TargetCheck()
    {
        if (gameManager.Target())
        {
            CheckAccess();
        }
        else
        {
            Debug.Log("Ÿ���� �����Ƿ� ���ٺҰ�");
        }
    }

    public void KeyCardCheck()
    {
        if (keycard.isHave)
        {
            gameManager.GetCard(keycard.AccessLevel, keycard.isHave);//Űī�带 ������������ ���ӸŴ������� ����
            Debug.Log(keycard.KeyColor + "key card�� ȹ���߽��ϴ�");
        }
        else
        {
            return;//����
        }
    }

    public void EndEvent()
    {
        isEvent = false;//�̺�Ʈ�� ����
        CanAccess = true;//Űī�忡 ���ٰ���
        Debug.Log("�̺�Ʈ�� �������ϴ�.\nŰī�忡 ���ΰ����մϴ�.");
    }

    private void StartEvent()
    {
        Event.SirenOn();//���̷� ���
        isEvent = true;
        Debug.Log("�̺�Ʈ�� ���۵ƽ��ϴ�.");
    }

    private void CheckAccess()
    {
        if(!isEvent && !CanAccess)//�̺�Ʈ�� ���������� �ʰ�, �������� �Ұ����ϴٸ�
        {
            StartEvent();//�̺�Ʈ �߻�!!
            Debug.Log("�̺�Ʈ ����");
        }
        else if (isEvent && !CanAccess)
        {
            Debug.Log("�̺�Ʈ ���̹Ƿ� ���� �ź�");
            return;//���� ���ϵ��� ����
        }
        else if(!isEvent && CanAccess)
        {
            keycard.AccessToKey();//Űī�� ȹ��
            KeyCardCheck();//Űī�带 ȹ�������� ���ӸŴ����� ����
            Debug.Log(accessLevel +"LvŰī�带 ȹ���߽��ϴ�.");
            gameObject.SetActive(false);//Űī�� ����(������Ʈ����)
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("�÷��̾ �������� ���� ����������Ʈ�� ���� ����");
        if (col.tag == "Player")
        {
            isPlayer = true;//������ Ȯ��(ù ���ٿ��� �̺�Ʈ ����, �̺�Ʈ �߿��� ���� �Ұ���, �̺�Ʈ ���� �Ŀ� Űī�� ȹ�� ����)
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isPlayer = false;//������ Ȯ��(ù ���ٿ��� �̺�Ʈ ����, �̺�Ʈ �߿��� ���� �Ұ���, �̺�Ʈ ���� �Ŀ� Űī�� ȹ�� ����)
        }
    }
}
