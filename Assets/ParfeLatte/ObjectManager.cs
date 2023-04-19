using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject EventMob;

    GameObject[] Mob;

    void Awake()
    {
        Mob = new GameObject[30];

        Generate();
    }

    void Generate()
    {
        for(int i = 0; i < Mob.Length; i++)
        {
            Mob[i] = Instantiate(EventMob);
            Mob[i].SetActive(false);
        }
    }

    public GameObject PullMob()
    {
        for(int i = 0; i < Mob.Length; i++)
        {
            if (!Mob[i].activeSelf)
            {
                Mob[i].SetActive(true);
                return Mob[i];
            }
        }
        return null;
    }
}
