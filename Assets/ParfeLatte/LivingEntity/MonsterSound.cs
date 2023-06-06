using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterSound : MonoBehaviour
{
    private AudioSource Audio;

    private bool isPlay;

    public AudioClip NormalMobSleep;
    public AudioClip GiantMobSleep;

    public AudioClip NormalMobAttack;
    public AudioClip GiantMobAttack;
    public AudioClip BossMobAttack;

    public AudioClip NormalMobWalk;
    public AudioClip GiantMobWalk;
    public AudioClip BossMobWalk;

    public AudioClip NormalMobDead;
    public AudioClip GiantMobDead1;
    public AudioClip GiantMobDead2;

    public AudioClip Damaged;
    // Start is called before the first frame update
    void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PlaySleepSound(string Type)
    {
        switch(Type){
            case "Normal":
                Audio.PlayOneShot(NormalMobSleep);
                break;
            case "Giant":
                Audio.PlayOneShot(GiantMobSleep);
                break;
        }
    }

    public void PlayAttackSound(string Type)
    {
        switch (Type)
        {
            case "Normal":
                Audio.PlayOneShot(NormalMobAttack);
                break;
            case "Giant":
                Audio.PlayOneShot(GiantMobAttack);
                break;
        }
    }

    public void PlayDeadSound(string Type)
    {
        switch (Type)
        {
            case "Normal":
                Audio.PlayOneShot(NormalMobDead);
                break;
            case "Giant":
                int i = Random.Range(0, 2);
                if (i == 0)
                {
                    Audio.PlayOneShot(GiantMobDead1);
                }
                else Audio.PlayOneShot(GiantMobDead1);
                break;
        }
    }

    public void PlayWalkSound(string Type)
    {
        switch (Type)
        {
            case "Normal":
                Audio.clip = NormalMobWalk;
                break;
            case "Giant":
                Audio.clip = GiantMobWalk;
                break;
        }
        if (isPlay) return;
        else if (!isPlay)
        {
            Audio.Play();
            Audio.loop = true;
            isPlay = true;
        }
    }

    public void StopSound()
    {
        if (!isPlay) return;
        else if (isPlay)
        {
            Audio.loop = false;
            isPlay = false;
        }
    }

    public void PlayDamagedSound()
    {
        Audio.PlayOneShot(Damaged);
    }
}
