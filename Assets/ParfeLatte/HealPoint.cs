using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    public Player player;

    public float Heal;

    private bool CanHeal;
    private int healCount;

    void Awake()
    {
        CanHeal = false;
        healCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if (healCount > 0)
        {
            if (CanHeal && Input.GetKeyDown(KeyCode.U))
            {
                if (player.Health < 100f)
                {
                    player.RestoreHealth(Heal);

                    healCount--;
                }
                else
                {
                    Debug.Log("최대체력이므로 회복하지 않습니다.");
                }
            }
        }

        if(healCount <= 0)
            return;

        if(player.Health >= 100f)
            return;

        if(CanHeal && Input.GetKeyDown(KeyCode.U)) {
            player.RestoreHealth(Heal);
            healCount--;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            CanHeal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            CanHeal = false;
        }
    }
}
