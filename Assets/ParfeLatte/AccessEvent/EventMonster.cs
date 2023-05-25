using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMonster : MonoBehaviour
{
    public Monster Mob;
    public bool isDead;

    private void Awake()
    {
        Mob.MonsterAwake();//몬스터 기상
        isDead = false;//안죽었음
    }
    private void OnEnable()
    {
        Mob.MonsterAwake();//몬스터 기상(오브젝트 풀링 준비)
        isDead = false;//안죽었음
    }

    private void OnDisable()
    {
        isDead = true;//사망
    }
}