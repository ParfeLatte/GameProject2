using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour
{
    public ParticleSystem Blood;
    private float firstPlaybackSpeed = 10.0f;
    private float SecPlaybackSpeed = 1.0f;
    private float TargetTIme = 1.0f;
    private bool isCheck;
    // Update is called once per frame

    private void Awake()
    {
        SetFirstSpeed();
        isCheck = false;
    }
    void Update()
    {
        if (!isCheck) return;
        float currentTime = Blood.main.duration - Blood.time;
        if(currentTime >= 1.0f)
        {
            SetSecondSpeed();
        }
    }

    public void ShowEffect()
    {
        SetFirstSpeed();
        Blood.Play();
        isCheck = true;
        Invoke("StopEffect", 1.3f);
    }

    private void StopEffect()
    {
        Blood.Stop();
        isCheck = false;
    }

    private void SetFirstSpeed()
    {
        var mainModule = Blood.main;
        mainModule.simulationSpeed = firstPlaybackSpeed;
    }

    private void SetSecondSpeed()
    {
        var mainModule = Blood.main;
        mainModule.simulationSpeed = SecPlaybackSpeed;
    }
}
