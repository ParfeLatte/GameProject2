using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insomnia;
public class OnLift : Interactable
{
    public LiftTest Lift;
    public override void OnInteractStart()
    {
        if (!Lift.isMove)
        {
            if (!Lift.Reverse)
            {
                Lift.Move();
                Lift.Up();
            }
            else
            {
                Lift.Move();
                Lift.Down();
            }
        }
    }

}
