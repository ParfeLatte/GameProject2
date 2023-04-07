using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessPoint : MonoBehaviour
{
    public KeyCard keycard;
    public int accessLevel;
    public bool isEvent = false;
    public event Action EventStart;
    public event Action EventClear;
    public AudioClip Siren;//���̷� ����

    private AudioSource SirenSound;//���̷� �����

    void Awake()
    {
        SirenSound = GetComponent<AudioSource>();
        accessLevel = keycard.AccessLevel;
        SirenSound.clip = Siren;

        EventStart += SirenOn;
        EventStart += MobSpawn;

        EventClear += keycard.AccessToKey;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SirenOn()
    {
        SirenSound.Play();
    }
    public void MobSpawn()
    {
        //���͵� ������Ŵ
    }
    
    public void KeyCardCheck()
    {
        if (keycard.isHave)
        {

        }
    }
}
