using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardEvent : MonoBehaviour
{
    public AccessPoint AP;//엑세스 포인트

    public MobSpawner Spawner;//몬스터 생성
    public Transform SpawnPos;//이벤트 몹 생성위치

    public int HowMuch;
    public int MobIndex;

    public float MaxPullPos;
    public float SpawnTimer;

    public bool isAllDead;//모든 몬스터가 죽었는지 확인

    private AudioSource SirenSound;//사이렌 재생용
    public Animator sirenAnimation;//사이렌 애니메이션

    public List<GameObject> MobList = new List<GameObject>();//몬스터 리스트
    public List<EventMonster> DeadCheck = new List<EventMonster>();//살았는지 체크
    void Awake()
    {
        SirenSound = GetComponent<AudioSource>();
        sirenAnimation.enabled = false;//비활성화
        isAllDead = false;//이벤트 몬스터가 아직 살아있음
        SpawnTimer = 0f;
        MobIndex = 0;
    }

    public void SirenOn()
    {
        SirenSound.Play();//사이렌 재생
        sirenAnimation.enabled = true;
    }
        
    public void SirenOff()
    {
        SirenSound.Stop();//사이렌 정지(이벤트 끝)
        sirenAnimation.enabled = false;
    }

    public void MobSpawn(int i)
    {
        //몬스터들 스폰시킴
        ChangePos();//랜덤한 위치에서 몹스폰
        MobList.Add(Spawner.spawnEnemy(SpawnPos));//정해진 위치에 몹 스폰
        DeadCheck.Add(MobList[i].GetComponent<EventMonster>());//이벤트몹 스크립트 가져옴
        SpawnTimer = 0;
        MobIndex++;
        Debug.Log("몬스터들이 몰려옵니다.");
    }

    private void ChangePos()
    {
        float xPos = UnityEngine.Random.Range(0f, 5f);//랜덤한 위치 하나 선정
        SpawnPos.position = SpawnPos.position + new Vector3(xPos, 0);
    } 

    public void ClearEvent()
    {
        SirenOff();//사이렌 정지
        AP.EndEvent();//이벤트 끝냄
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
                isAllDead = false;//몬스터가 한 마리라도 살아있으면 false가 반환됨
                return;
            }
        }
        if (isAllDead)
        {
            ClearEvent();//몬스터가 전부 죽었으면 이벤트 끝
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
        }//이벤트 중일때 이벤트 몹의 생사 확인
    }
}