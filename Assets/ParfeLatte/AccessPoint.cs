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

    public bool isEvent = false;//�̺�Ʈ�� ����������
    public bool CanAccess = false;//ī�忡 ���ٰ�������

    public event Action EventStart;

    void Awake()
    {
        EventStart += SetEvent;
        EventStart += Event.SirenOn;
        EventStart += Event.MobSpawn;
    }

    void Update()
    {
        
    }

    private void SetEvent()
    {
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
            EventStart();//�̺�Ʈ �߻�!!
            Debug.Log("�̺�Ʈ ����");
        }
        else if (isEvent)
        {
            Debug.Log("�̺�Ʈ ���̹Ƿ� ���� ����");
            return;
        }
        else if(!isEvent && CanAccess)
        {
            keycard.AccessToKey();
            KeyCardCheck();
            Debug.Log("Űī�忡 ���� ����");
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
       if(col.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                CheckAccess();//������ Ȯ��(ù ���ٿ��� �̺�Ʈ ����, �̺�Ʈ �߿��� ���� �Ұ���, �̺�Ʈ ���� �Ŀ� Űī�� ȹ�� ����)
            }
        }
    }
}
