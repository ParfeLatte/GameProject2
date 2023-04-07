using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardEvent : MonoBehaviour
{
    public AccessPoint AP;//������ ����Ʈ
    public GameObject Monster;//���� ������ ���͵�!

    public AudioClip Siren;//���̷� ����

    public Transform SpawnPos;//�̺�Ʈ �� ������ġ

    private AudioSource SirenSound;//���̷� �����

    public List<Monster> MobList = new List<Monster>();//����ִ��� üũ��
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
        //���͵� ������Ŵ
        for(int i = 0; i < 5; i++)
        {
            GameObject monster = Instantiate(Monster, SpawnPos);
            Monster Mob = monster.GetComponent<Monster>();
            MobList[i] = Mob;
        }
        Debug.Log("���͵��� �����ɴϴ�.");
    }

    public void CheckMob()
    {
        for(int i = 0; i < MobList.Count; i++)
        {
            if (MobList[i].isDead) { MobList.RemoveAt(i); }
        }
        //�̺�Ʈ ���� ����ִ��� Ȯ����
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
