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
                    Debug.Log("�ִ�ü���̹Ƿ� ȸ������ �ʽ��ϴ�.");
                }
            }
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
