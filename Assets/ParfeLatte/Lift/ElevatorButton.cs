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
        if (isKey)
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

    public override void OnInteractStart()
    {
        if (!elevator.isMove && !isKey)
        {
            isKey = true;
            Fkey.HideInteractUI();
            MoveInform.SetActive(true);
        }
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
        Debug.Log("W키를 눌러 위로, S키를 눌러 아래로 이동하세요");
    }
}
