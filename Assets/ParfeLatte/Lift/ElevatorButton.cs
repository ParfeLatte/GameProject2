using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;

public class ElevatorButton : Interactable 
{
    public Elevator elevator;
    public bool isKey;

    private void Update()
    {
        if (isKey)
        {
            CheckUpDown();
        }
    }

    public override void OnInteractStart()
    {
        if (!elevator.isMove && !isKey)
        {
            isKey = true;
        }
    }

    private void CheckUpDown()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            elevator.Up();
            elevator.Move();
            isKey = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            elevator.Down();
            elevator.Move();
            isKey = false;
        }
        Debug.Log("W키를 눌러 위로, S키를 눌러 아래로 이동하세요");
    }
}
