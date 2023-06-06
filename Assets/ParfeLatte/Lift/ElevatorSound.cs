using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ElevatorSound : MonoBehaviour
{
    public AudioClip ElevatorOpen;
    public AudioClip ElevatorClose;
    public AudioClip ElevMoveSound;

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

    public void OpenSound()
    {
        Audio.PlayOneShot(ElevatorOpen);
    }

    public void CloseSound()
    {
        Audio.PlayOneShot(ElevatorClose);
    }

    public void MoveSound()
    {
        Audio.clip = ElevMoveSound;
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
            Audio.Stop();
            Audio.loop = false;
            isPlay = false;
        }
    }
}
