using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateHP : LivingEntity
{
    public GateCheck Gate;
    // Start is called before the first frame update
    void Awake()
    {
        SetStatus(200, 0, 0);
        Health = MaxHealth;
        isDead = false;
    }


    public override void Die()
    {
        base.Die();
        Gate.DestroyGate();
    }


}
