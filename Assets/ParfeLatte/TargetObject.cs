using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : SearchableBase
{
    public GameManager GameMgr;

    private void Awake()
    {
        base.Awake();
        if (GameMgr.isTarget)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    protected override void Start() {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        { 
            GameMgr.GetTarget();
            gameObject.SetActive(false);
        }
    }
}
