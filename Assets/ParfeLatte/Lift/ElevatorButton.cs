using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;

public class ElevatorButton : Interactable 
{
    public Elevator elevator;
    public GameObject MoveInform;
    public InteractObj Fkey;
    public bool isKey;

    private void Update()
    {
        if(GameManager.IsPause)
            return;

        if(isKey)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ElevUp();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                ElevDown();
            }
        }
    }

    public override bool OnInteractStart()
    {
        if (!elevator.isMove && !isKey)
        {
            isKey = true;
            Fkey.HideInteractUI();
            MoveInform.SetActive(true);
        }

        return true;
    }

    private void ElevUp()
    {
        MoveInform.SetActive(false);
        isKey = false;
        elevator.Up();
        
    }
    
    private void ElevDown()
    {
        MoveInform.SetActive(false);
        isKey = false;
        elevator.Down();
        
    }
    private void CheckUpDown()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isKey = false;
            elevator.Up();
            MoveInform.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            isKey = false;
            elevator.Down();
            MoveInform.SetActive(false);
        }
    }
}
