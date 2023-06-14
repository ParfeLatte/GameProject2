using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Insomnia;

public class ChangeCam : ImmortalSingleton<ChangeCam>
{
    [SerializeField]
    private CinemachineVirtualCamera PlayerCam;
    [SerializeField]
    private CinemachineVirtualCamera BossMobCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    SwitchCam();
        //}
    }

    public void SwitchCam()
    {
        if (PlayerCam.enabled)
        {
            BossMobCam.enabled = true;
            PlayerCam.enabled = false;
        }
        else
        {
            PlayerCam.enabled = true;
            BossMobCam.enabled = false;
        }
    }
}
