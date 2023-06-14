using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

public class HealPoint : SearchableBase {
    public Player player;

    public float Heal;

    private bool m_canHeal;
    private int m_healCount;

    public bool CanHeal { get => m_canHeal; }
    public int HealCount { get => m_healCount; }

    protected override void Awake()
    {
        base.Awake();
        m_canHeal = false;
        m_healCount = 2;
    }

    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if(m_healCount > 0)
        {
            if (m_canHeal && Input.GetKeyDown(KeyCode.F))
            {
                if (player.Health < 100f)
                {
                    player.RestoreHealth(Heal);

                    m_healCount--;
                }
                else
                {

                }
            }
        }

        if(m_healCount <= 0)
            return;

        if(player.Health >= 100f)
            return;

        if(m_canHeal && Input.GetKeyDown(KeyCode.U)) {
            player.RestoreHealth(Heal);
            m_healCount--;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            m_canHeal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            m_canHeal = false;
        }
    }

    protected override void Reset() {
        
    }
}
