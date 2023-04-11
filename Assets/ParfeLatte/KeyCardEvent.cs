using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardEvent : MonoBehaviour
{
    public AccessPoint AP;//엑세스 포인트

    public MobSpawner Spawner;//몬스터 생성
    public List<Transform> SpawnPos = new List<Transform>();//이벤트 몹 생성위치

    public bool isAllDead;//모든 몬스터가 죽었는지 확인

    private AudioSource SirenSound;//사이렌 재생용


    public List<GameObject> MobList = new List<GameObject>();//몬스터 리스트
    public List<EventMonster> DeadCheck = new List<EventMonster>();//살았는지 체크
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
        //몬스터들 스폰시킴
        for(int i = 0; i < 5; i++)
        {
            int pos = ChangePos();
            MobList.Add(Spawner.spawnEnemy(SpawnPos[pos]));
            DeadCheck.Add(MobList[i].GetComponent<EventMonster>());
        }
        Debug.Log("몬스터들이 몰려옵니다.");
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