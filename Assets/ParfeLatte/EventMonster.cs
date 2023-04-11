using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMonster : MonoBehaviour
{
    public Monster Mob;
    public bool isDead;

    private void Awake()
    {
        Mob.MonsterAwake();
        isDead = false;
    }
    private void OnEnable()
    {
        Mob.MonsterAwake();
        isDead = false;
    }

    private void OnDisable()
    {
        isDead = true;
    }
}