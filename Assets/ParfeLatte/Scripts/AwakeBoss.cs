using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.BGM_Speaker;

public class AwakeBoss : MonoBehaviour
{
    public GameObject Boss;

    public void BossEvent()
    {
        BGM_Speaker.Instance.Play((int)BGMSounds.Boss, true);
        Boss.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Invoke("BossEvent", 0.4f);
        }
    }
}
