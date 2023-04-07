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
    public AudioClip Siren;//사이렌 사운드

    private AudioSource SirenSound;//사이렌 재생용

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
        //몬스터들 스폰시킴
    }
    
    public void KeyCardCheck()
    {
        if (keycard.isHave)
        {

        }
    }
}
