using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessPoint : MonoBehaviour
{
    public KeyCard keycard;
    public GameManager gameManager;
    public KeyCardEvent Event;

    public int accessLevel;

    public bool isEvent;//�̺�Ʈ�� ����������
    public bool isEnd;//�̺�Ʈ�� ���´���
    public bool CanAccess;//ī�忡 ���ٰ�������

    void Awake()
    {
        isEvent = false;
        isEnd = false;
        CanAccess = false;
    }

    void Update()
    {
        
    }

    private void StartEvent()
    {
        Event.SirenOn();
        Event.MobSpawn();
        isEvent = true;
        Debug.Log("�̺�Ʈ�� ���۵ƽ��ϴ�.");
    }

    public void EndEvent()
    {
        isEvent = false;
        CanAccess = true;
        Debug.Log("�̺�Ʈ�� �������ϴ�.\nŰī�忡 ���ΰ����մϴ�.");
    }

    public void KeyCardCheck()
    {
        if (keycard.isHave)
        {
            gameManager.GetCard(keycard.AccessLevel, keycard.isHave);
            Debug.Log(keycard.KeyColor + "key card�� ȹ���߽��ϴ�");
        }
        else
        {
            return;
        }
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
            return;
        }
        else if(!isEvent && CanAccess)
        {
            keycard.AccessToKey();
            KeyCardCheck();
            Debug.Log("Űī�忡 ���� ����");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("�÷��̾ �������� ���� ����������Ʈ�� ���� ����");
        if (col.tag == "Player" && Input.GetKeyDown(KeyCode.U))
        {
            CheckAccess();//������ Ȯ��(ù ���ٿ��� �̺�Ʈ ����, �̺�Ʈ �߿��� ���� �Ұ���, �̺�Ʈ ���� �Ŀ� Űī�� ȹ�� ����)
        }
    }
}
