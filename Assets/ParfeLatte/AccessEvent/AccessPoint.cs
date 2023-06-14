using Insomnia;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

[Obsolete("No Use Anymore", true)]
public class AccessPoint : SearchableBase {
    public KeyCard keycard;
    public KeyCardEvent Event;

    public int accessLevel; //Űī�� ����

    public bool isEvent;    //�̺�Ʈ�� ����������
    public bool isEnd;      //�̺�Ʈ�� ���´���
    public bool CanAccess;  //ī�忡 ���ٰ�������

    private bool isPlayer;  //�÷��̾ �����ߴ���

    protected override void Awake()
    {
        base.Awake();
        isEvent = false;
        isEnd = false;
        CanAccess = false;
        isPlayer = false;
        accessLevel = keycard.AccessLevel;
    }

    protected override void Start() {
        base.Start();
    }

    void Update()
    {
        if(GameManager.IsPause)
            return;

        if(isPlayer && Input.GetKeyDown(KeyCode.F)){
            switch (accessLevel)
            {
                case 4:
                    TargetCheck();
                    break;
                default:
                    CheckAccess();//�÷��̾ ���� ���� ������ F�� �������� ���ٽõ�
                    break;
            }
        }
    }

    public void TargetCheck()
    {
        //if (gameManager.Target())
        //{
        //    CheckAccess();
        //}
        //else
        //{
        //    Debug.Log("Ÿ���� �����Ƿ� ���ٺҰ�");
        //}
    }

    public void KeyCardCheck()
    {
        //if (keycard.isHave)
        //{
        //    gameManager.GetCard(keycard.AccessLevel, keycard.isHave);//Űī�带 ������������ ���ӸŴ������� ����
        //    Debug.Log(keycard.KeyColor + "key card�� ȹ���߽��ϴ�");
        //}
        //else
        //{
        //    return;//����
        //}
    }

    public void EndEvent()
    {
        isEvent = false;//�̺�Ʈ�� ����
        CanAccess = true;//Űī�忡 ���ٰ���
    }

    private void StartEvent()
    {
        Event.SirenOn();//���̷� ���
        isEvent = true;
    }

    private void CheckAccess()
    {
        if(!isEvent && !CanAccess)//�̺�Ʈ�� ���������� �ʰ�, �������� �Ұ����ϴٸ�
        {
            StartEvent();//�̺�Ʈ �߻�!!
        }
        else if (isEvent && !CanAccess)
        {
            return;//���� ���ϵ��� ����
        }
        else if(!isEvent && CanAccess)
        {
            keycard.AccessToKey();//Űī�� ȹ��
            KeyCardCheck();//Űī�带 ȹ�������� ���ӸŴ����� ����
            gameObject.SetActive(false);//Űī�� ����(������Ʈ����)
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
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

    protected override void Reset() {
        
    }
}
