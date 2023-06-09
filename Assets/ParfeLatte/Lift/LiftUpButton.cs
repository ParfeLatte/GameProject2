using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;

public class LiftUpButton : Interactable
{
    public LiftTest Lift;
    public override bool OnInteractStart()
    {
        if (Lift.Reverse)
        {
            return true;
        }
        else
        {
            Lift.Move();
            Lift.Up();
            return true;
        }
    }
}
