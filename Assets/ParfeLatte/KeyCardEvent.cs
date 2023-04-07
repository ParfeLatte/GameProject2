using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardEvent : MonoBehaviour
{
    public AccessPoint AP;//엑세스 포인트
    public GameObject Monster;//마구 몰려올 몬스터들!

    public AudioClip Siren;//사이렌 사운드

    public Transform SpawnPos;//이벤트 몹 생성위치

    private AudioSource SirenSound;//사이렌 재생용

    public List<Monster> MobList = new List<Monster>();//살아있는지 체크함
    void Start()
    {
        SirenSound = GetComponent<AudioSource>();
        SirenSound.clip = Siren;
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
            GameObject monster = Instantiate(Monster, SpawnPos);
            Monster Mob = monster.GetComponent<Monster>();
            MobList[i] = Mob;
        }
        Debug.Log("몬스터들이 몰려옵니다.");
    }

    public void CheckMob()
    {
        for(int i = 0; i < MobList.Count; i++)
        {
            if (MobList[i].isDead) { MobList.RemoveAt(i); }
        }
        //이벤트 몹이 살아있는지 확인함
        if(MobList.Count == 0)
        {
            SirenOff();
            AP.EndEvent();
        }
    }

    void Update()
    {
        CheckMob();
    }

    
}
