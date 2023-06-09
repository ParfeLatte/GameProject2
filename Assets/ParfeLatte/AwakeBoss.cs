using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeBoss : MonoBehaviour
{
    public GameObject Boss;

    public void BossEvent()
    {
        Boss.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ChangeCam.Instance.SwitchCam();
            Invoke("BossEvent", 0.4f);
        }
    }
}
