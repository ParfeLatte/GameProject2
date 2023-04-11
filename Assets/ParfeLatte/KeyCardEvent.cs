using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardEvent : MonoBehaviour
{
    public AccessPoint AP;//������ ����Ʈ

    public MobSpawner Spawner;//���� ����
    public List<Transform> SpawnPos = new List<Transform>();//�̺�Ʈ �� ������ġ

    public bool isAllDead;//��� ���Ͱ� �׾����� Ȯ��

    private AudioSource SirenSound;//���̷� �����


    public List<GameObject> MobList = new List<GameObject>();//���� ����Ʈ
    public List<EventMonster> DeadCheck = new List<EventMonster>();//��Ҵ��� üũ
    void Awake()
    {
        SirenSound = GetComponent<AudioSource>();
        isAllDead = false;
    }

    public void SirenOn()
    {
        SirenSound.Play();
    }
        
    public void SirenOff()
    {
        SirenSound.Stop();
    }

    public void MobSpawn()
    {
        //���͵� ������Ŵ
        for(int i = 0; i < 5; i++)
        {
            int pos = ChangePos();
            MobList.Add(Spawner.spawnEnemy(SpawnPos[pos]));
            DeadCheck.Add(MobList[i].GetComponent<EventMonster>());
        }
        Debug.Log("���͵��� �����ɴϴ�.");
    }

    private int ChangePos()
    {
        return UnityEngine.Random.Range(0, 4);
    } 

    public void ClearEvent()
    {
        SirenOff();
        AP.EndEvent();
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
                isAllDead = false;
                return;
            }
        }
        if (isAllDead)
        {
            ClearEvent();
        }

    }

    void Update()
    {
        if (AP.isEvent)
        {
            CheckMob();
        }
    }
}