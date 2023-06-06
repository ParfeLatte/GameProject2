    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    public AudioClip WalkAudio;

    public AudioClip NormalMob;
    public AudioClip GiantMob;

    private AudioSource Audio;

    private bool isPlay;
    // Start is called before the first frame update
    void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DamagedSound(string MobType)
    {
        switch (MobType)
        {
            case "Normal":
                Audio.PlayOneShot(NormalMob);
                break;
            case "Giant":
                Audio.PlayOneShot(GiantMob);
                break;
        }
    }

    public void MoveSound()
    {
        Audio.clip = WalkAudio;
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
        else if(isPlay)
        {
            Audio.loop = false;
            isPlay = false;
        }
    }
}
