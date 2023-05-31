using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;

public class ElevatorButton : Interactable 
{
    public Elevator elevator;
    public GameObject MoveInform;
    public bool isKey;

    private void Update()
    {
        if(GameManager.IsPause)
            return;

        if(isKey)
        {
            CheckUpDown();
        }
    }

    public override void OnInteractStart()
    {
        if (!elevator.isMove && !isKey)
        {
            isKey = true;
            MoveInform.SetActive(true);
        }
    }

    private void CheckUpDown()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            elevator.Up();
            isKey = false;
            MoveInform.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            elevator.Down();
            isKey = false;
            MoveInform.SetActive(false);
        }
        Debug.Log("W키를 눌러 위로, S키를 눌러 아래로 이동하세요");
    }
}
