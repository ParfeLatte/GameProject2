using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorSound : MonoBehaviour
{
    public AudioClip Open;
    public AudioClip Close;

    private AudioSource Audio;
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
        Audio.PlayOneShot(Open);
    }

    public void CloseSound()
    {
        Audio.PlayOneShot(Close);
    }
}
