using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;
public class LiftDownButton : Interactable
{
    public LiftTest Lift;

    public override void OnInteractStart()
    {
        if(!Lift.Reverse)
        {
            return;
        }
        else
        {
            Lift.Move();
            Lift.Down();
        }
    }
}
