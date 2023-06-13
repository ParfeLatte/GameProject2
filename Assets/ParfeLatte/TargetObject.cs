using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

public class TargetObject : SearchableBase {
    public GameManager GameMgr;

    protected override void Awake() {
        base.Awake();
        if(GameMgr.isTarget) {
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
        }
    }

    protected override void Start() {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            GameMgr.GetTarget();
            gameObject.SetActive(false);
        }
    }

    protected override void Reset() {
        m_IDFormat = "TARGET";
        m_Location = "ZONE_";
        m_ObjectType = ObjectType.Resources;
        m_Status = StatusType.Normal;
    }
}
