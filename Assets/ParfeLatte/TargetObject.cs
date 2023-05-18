using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public GameManager GameMgr;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        { 
            GameMgr.GetTarget();
            gameObject.SetActive(false);
        }
    }
}
