using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardEvent : MonoBehaviour
{
    public AccessPoint AP;//������ ����Ʈ

    public MobSpawner Spawner;//���� ����
    public Transform SpawnPos;//�̺�Ʈ �� ������ġ

    public int HowMuch;
    public int MobIndex;

    public float MaxPullPos;
    public float SpawnTimer;

    public bool isAllDead;//��� ���Ͱ� �׾����� Ȯ��

    private AudioSource SirenSound;//���̷� �����
    public Animator sirenAnimation;//���̷� �ִϸ��̼�

    public List<GameObject> MobList = new List<GameObject>();//���� ����Ʈ
    public List<EventMonster> DeadCheck = new List<EventMonster>();//��Ҵ��� üũ
    void Awake()
    {
        SirenSound = GetComponent<AudioSource>();
        sirenAnimation.enabled = false;//��Ȱ��ȭ
        isAllDead = false;//�̺�Ʈ ���Ͱ� ���� �������
        SpawnTimer = 0f;
        MobIndex = 0;
    }

    public void SirenOn()
    {
        SirenSound.Play();//���̷� ���
        sirenAnimation.enabled = true;
    }
        
    public void SirenOff()
    {
        SirenSound.Stop();//���̷� ����(�̺�Ʈ ��)
        sirenAnimation.enabled = false;
    }

    public void MobSpawn(int i)
    {
        //���͵� ������Ŵ
        ChangePos();//������ ��ġ���� ������
        MobList.Add(Spawner.spawnEnemy(SpawnPos));//������ ��ġ�� �� ����
        DeadCheck.Add(MobList[i].GetComponent<EventMonster>());//�̺�Ʈ�� ��ũ��Ʈ ������
        SpawnTimer = 0;
        MobIndex++;
        Debug.Log("���͵��� �����ɴϴ�.");
    }

    private void ChangePos()
    {
        float xPos = UnityEngine.Random.Range(0f, 5f);//������ ��ġ �ϳ� ����
        SpawnPos.position = SpawnPos.position + new Vector3(xPos, 0);
    } 

    public void ClearEvent()
    {
        SirenOff();//���̷� ����
        AP.EndEvent();//�̺�Ʈ ����
    }

    public void CheckMob()
    {
        for(int i = 0; i < MobList.Count; i++)
        {
            if (DeadCheck[i].isDead)
            {
                isAllDead = true;
            }
            else
            {
                isAllDead = false;//���Ͱ� �� ������ ��������� false�� ��ȯ��
                return;
            }
        }
        if (isAllDead)
        {
            ClearEvent();//���Ͱ� ���� �׾����� �̺�Ʈ ��
        }

    }

    void Update()
    {
        if(GameManager.IsPause)
            return;

        SpawnTimer += Time.deltaTime;
        if (AP.isEvent)
        {
            if (SpawnTimer >= 0.8f && MobIndex < HowMuch)
            {
                MobSpawn(MobIndex);
            }
            CheckMob();
        }//�̺�Ʈ ���϶� �̺�Ʈ ���� ���� Ȯ��
    }
}