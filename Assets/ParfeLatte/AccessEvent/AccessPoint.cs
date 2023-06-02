using Insomnia;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

public class AccessPoint : SearchableBase, IDataIO
{
    public KeyCard keycard;
    public GameManager gameManager;
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
        LoadData();
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
        SaveData();
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
            SaveData();
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
            SaveData();
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

    private void OnApplicationQuit() {
        RemoveData();
    }

    public void SaveData() {
        string jsonData = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(gameObject.name, jsonData );
        PlayerPrefs.Save();


        //public bool isEvent;
        //public bool isEnd;
        //public bool CanAccess;

        PlayerPrefs.SetInt(gameObject.name, 1);
        PlayerPrefs.SetInt(gameObject.name + "_isEvent", isEvent ? 1 : 0);
        PlayerPrefs.SetInt(gameObject.name + "_isEnd", isEnd ? 1 : 0);
        PlayerPrefs.SetInt(gameObject.name + "_CanAccess", CanAccess ? 1 : 0);
        PlayerPrefs.SetInt(gameObject.name + "_activeSelf", gameObject.activeSelf ? 1 : 0);
        PlayerPrefs.Save();
}

public void LoadData() {
        if(PlayerPrefs.HasKey(gameObject.name) == false) 
            return;

        gameObject.SetActive(PlayerPrefs.GetInt(gameObject.name + "_activeSelf") == 1 ? true : false);
        isEvent = PlayerPrefs.GetInt(gameObject.name + "_isEvent") == 1 ? true : false;
        isEnd = PlayerPrefs.GetInt(gameObject.name + "_isEnd") == 1 ? true : false;
        CanAccess = PlayerPrefs.GetInt(gameObject.name + "_CanAccess") == 1 ? true : false;
    }

    public void RemoveData() {
        PlayerPrefs.DeleteKey(gameObject.name);
        PlayerPrefs.Save();
    }
}
