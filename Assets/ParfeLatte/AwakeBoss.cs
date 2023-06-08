using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeBoss : MonoBehaviour
{
    public GameObject Boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Boss.SetActive(true);
        }
    }
}
