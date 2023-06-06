using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;

public class ElevatorCall : Interactable
{
    public Elevator elevator;
    public int floor;

    public override void OnInteractStart()
    {
        elevator.Call(floor);
    }
}
