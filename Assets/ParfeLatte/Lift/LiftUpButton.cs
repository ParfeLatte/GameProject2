using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;

public class LiftUpButton : Interactable
{
    public LiftTest Lift;
    public override void OnInteractStart()
    {
        if (Lift.Reverse)
        {
            return;
        }
        else
        {
            Lift.Move();
            Lift.Up();
        }
    }
}
