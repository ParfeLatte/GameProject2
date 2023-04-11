using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCheck : MonoBehaviour
{
    public GameObject Gate;
    public GameManager Manager;

    public bool isOpen;
    public int GateLv;

    private SpriteRenderer Spr;

    private void Awake()
    {
        Spr = GetComponent<SpriteRenderer>();
        Gate.SetActive(true);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            Gate.SetActive(false);
            Spr.color = new Color(1, 1, 1, 0.4f);
        }
        else
        {
            Gate.SetActive(true);
            Spr.color = new Color(1, 1, 1, 1f);
        }
     }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isOpen = Manager.CheckGateOpen(GateLv);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isOpen = false;
        }
    }
}
