using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;
public class LiftDownButton : Interactable
{
    public LiftTest Lift;

    public override bool OnInteractStart()
    {
        if(!Lift.Reverse)
        {
            return true;
        }
        else
        {
            Lift.Move();
            Lift.Down();
            return true;
        }
    }
}
