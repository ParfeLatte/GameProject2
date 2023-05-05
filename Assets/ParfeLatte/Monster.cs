using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    //public GameObject small;//¹üÀ§ 15ÀÇ Äİ¶óÀÌ´õ ¿µ¿ª
    //public GameObject Big;//¹üÀ§ 30ÀÇ Äİ¶óÀÌ´õ ¿µ¿ª
    public GameObject Player;//°Å¸®ÃøÁ¤¿ë
    public Transform AttackPos;//°ø°İ¹üÀ§ À§Ä¡
    public Vector2 boxSize;//°ø°İ¹üÀ§

    public int SleepState;//ÇöÀç ¼ö¸é»óÅÂ 0:±íÀº¼ö¸é, 1:Áß°£¼ö¸é, 2:¾èÀº¼ö¸é, 3:±â»ó!!!

    public float moveTimeOne;//ÇÃ·¹ÀÌ¾î°¡ ¾ó¸¶³ª °É¾ú´ÂÁö È®ÀÎ
    //public float moveTimeTwo;//ÇÃ·¹ÀÌ¾î°¡ ¾ó¸¶³ª °É¾ú´ÂÁö È®ÀÎ
    public float CheckTime;//¹üÀ§ ¾È¿¡ µé¾î¿Â ½Ã°£
    public float CoolTime;//°ø°İ ÄğÅ¸ÀÓ
    public float AttackTime;//°ø°İ½Ã°£
    public float Dist;//¸ó½ºÅÍ¿Í ÇÃ·¹ÀÌ¾î »çÀÌ °Å¸®
    public float Dir;//ÀÌµ¿¹æÇâ
    public Vector3 dirVec;//·¹ÀÌ ¹æÇâ

    public Player player;//ÇÃ·¹ÀÌ¾î ÄÚµå
    //public MonsterAttack Attack;//°ø°İ ½ÇÇà ½ºÅ©¸³Æ®

    public bool isPlayerDash;//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬Çß´ÂÁö È®ÀÎ
    public bool isPlayerStop;//ÇÃ·¹ÀÌ¾î°¡ ¸ØÃè´Â°¡?
    public bool isAttack;//°ø°İÁßÀÎ°¡?
    public bool isWall;//º®¿¡ ºÎµúÇû´Â°¡?

    public Vector3 lastPlayerPosition;//À§Ä¡ ºñ±³¿ë


    private Rigidbody2D MR;
    private Animator animator;
    private SpriteRenderer MonsterRenderer;

    private Vector3 curPos;//ÇöÀçÀ§Ä¡
    private Vector2 AddPos = new Vector2(0, 3f);//pivotÀ» ¾Æ·¡·Î °íÁ¤ÇßÀ¸¹Ç·Î ·¹ÀÌ °Ë»ç¶§ À§·Î
    private Vector2 RayPos;//·¹ÀÌ ¹æÇâ
    private GameObject AttackPlayer;//°ø°İ´ë»ó

    private bool isMobMove = false;//¸ó½ºÅÍ°¡ ¿òÁ÷ÀÌ´ÂÁö

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MonsterRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.Find("Player");
        SleepState = 0;//±íÀº¼ö¸é »óÅÂ·Î ½ºÆù
        SetStatus(30, 10, 4.5f);//ÀÏ´Ü ÀÏ¹İ¸÷±âÁØ
        Health = MaxHealth;//Ã¼·ÂÀ» ÃÖ´ëÃ¼·ÂÀ¸·Î ¼³Á¤ 
    }

    private void OnEnable()
    {
        isDead = false;
        SetStatus(30, 10, 4.5f);//ÀÏ´Ü ÀÏ¹İ¸÷±âÁØ
        Health = MaxHealth;//Ã¼·ÂÀ» ÃÖ´ëÃ¼·ÂÀ¸·Î ¼³Á¤ 
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//ÇÃ·¹ÀÌ¾î¿Í ¸ó½ºÅÍ »çÀÌ °Å¸®
        if (Mathf.Abs(Player.transform.position.y - gameObject.transform.position.y) < 5)
        {
            if (SleepState != 3 & !isDead)
            {
                StateCheck();//ÇöÀç ¼ö¸é»óÅÂÀÌ¹Ç·Î ¼ö¸é»óÅÂ È®ÀÎ
            }//¼ö¸é½Ã ÆĞÅÏ
        }
        curPos = transform.position;//ÇöÀçÀ§Ä¡
        RayPos = MR.position + AddPos;//·¹ÀÌ¸¦ ¹ß»çÇÏ´Â À§Ä¡
        if (SleepState == 3 && !isDead) {
            OnWake();//±ú¾îÀÖÀ»¶§ Çàµ¿ÆĞÅÏ
            AttackCheck();//°ø°İ¹üÀ§ ³»¿¡ µé¾î¿À¸é °ø°İ
            AttackTime += Time.deltaTime;//ÄğÅ¸ÀÓ
        }//±â»ó½Ã ÆĞÅÏ
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(AttackPos.position, boxSize);
    //}

    private void AttackCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(RayPos, dirVec, 1.75f, LayerMask.GetMask("Player"));//Ray¸¦ ¹ß»çÇÏ¿© ÇÃ·¹ÀÌ¾î°¡ ¹üÀ§³»¿¡ ÀÖ´ÂÁö È®ÀÎ
        if (rayHit.collider != null)//·¹ÀÌ¿¡ Ãæµ¹ÇÑ ´ë»óÀÌ ÀÖÀ¸¸é
        {
            Player player = rayHit.collider.GetComponent<Player>();//ÇÃ·¹ÀÌ¾î ÄÄÆ÷³ÍÆ® ÇÒ´ç
            AttackPlayer = rayHit.collider.gameObject;//°ø°İÇÒ ÇÃ·¹ÀÌ¾î ´ë»ó ÇÒ´ç
            //Debug.Log("ÇÃ·¹ÀÌ¾î°¡ »çÁ¤°Å¸®³»¿¡ ÀÖÀ½");
            if (AttackPlayer != null && AttackTime >= CoolTime)//°ø°İ ÄğÅ¸ÀÓÀÌ µ¹¾Ò°í, ÇÃ·¹ÀÌ¾î°¡ ÇÒ´çµÇ¾îÀÖ´Ù¸é
            {
                isMobMove = false;//Àá½Ã ¿òÁ÷ÀÓÀ» ¸ØÃß°í
                animator.SetBool("isMove", false);//¾Ö´Ï¸ŞÀÌ¼Ç ÆÄ¶ó¹ÌÅÍ¿¡¼­ isMove¸¦ false·Î ¹Ù²Ù°í
                animator.SetTrigger("Attack");//attack Trigger¸¦ ¹ßµ¿ÇØ¼­ °ø°İ ¾Ö´Ï¸ŞÀÌ¼Ç Àç»ı
                Invoke("Attack", 0.38f);//¾Ö´Ï¸ŞÀÌ¼Ç¿¡¼­ ÈÖµÎ¸£´Â ¸ğ¼Ç¿¡ ¸Â°Ô °ø°İ
                AttackTime = 0;//´Ù½Ã ÄğÅ¸ÀÓ
                Invoke("MoveAgain", 0.55f);//°ø°İÈÄ¿¡ ´Ù½Ã ¿òÁ÷ÀÌµµ·Ï
            }
            //Debug.Log("°ø°İÇÕ´Ï´Ù.");
        }
        else
        {
            AttackPlayer = null;
            //Debug.Log("»çÁ¤°Å¸®³»¿¡ ¾Æ¹«µµ ¾øÀ½");
            //Debug.Log("°è¼Ó ÃßÀûÇÕ´Ï´Ù");
        }//¿¹¿ÜÃ³¸®
    }//°ø°İ

    private void Attack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(AttackPos.position, boxSize, 0);//¿À¹ö·¦ ¹Ú½º¸¦ ÀÌ¿ë
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")//¿À¹ö·¦¹Ú½º·Î °Ë»çÇÑ ¿ÀºêÁ§Æ® Áß¿¡¼­ ÇÃ·¹ÀÌ¾î°¡ ÀÖÀ¸¸é
            {
                player.damaged(damage);//ÇÃ·¹ÀÌ¾îÀÇ damaged ÇÔ¼ö È£ÃâÇØ¼­ µ¥¹ÌÁö¸¦ ÁÜ
            }
        }
    }//¹üÀ§³» °ø°İ

    public void MoveAgain()
    { 
        animator.SetBool("isMove", true);//ÀÌµ¿ ¾Ö´Ï¸ŞÀÌ¼Ç ¼Â
        isMobMove = true;//´Ù½Ã ¿òÁ÷ÀÓ
    }

    private void StateCheck()
    {
        if (SleepState != 3)
        {
            if (SleepState == 1)
            {
                moveTimeOne += Time.deltaTime;//20ÀÇ ¹üÀ§³»¿¡ ÀÖÀ»¶§ °Ë»çÇÏ´Â ½Ã°£
            }//±íÀº ¼ö¸éÀÏ¶§ °Ë»çÇÏ´Â ½Ã°£
            if (!player.isMove)
            {
                StartCoroutine("CheckStop");//ÇÃ·¹ÀÌ¾îÀÇ ¿òÁ÷ÀÓÀÌ ¸ØÃè´ÂÁö °Ë»ç
            }//¸ØÃèÀ¸¸é ¸ØÃã°Ë»ç
            else
            {
                isPlayerStop = false;
            }//¸ØÃßÁö ¾Ê¾ÒÀ½
            AreaCheck(SleepState);//¼ö¸é°Ë»ç!
        }
    }
    private void OnWake()
    {
        Debug.DrawRay(RayPos, dirVec * 1.75f, new Color(0, 1, 0));//·¹ÀÌÄ³½ºÆ® Ç¥½Ã(°Å¸® È®ÀÎ¿ë)
        if (isMobMove == true)
        {
            
            if (player.transform.position.x - gameObject.transform.position.x > 0)
            {
                Dir = 1;//¹æÇâ ¿À¸¥ÂÊ
                dirVec = new Vector3(1f, 0f, 0f);//Ray ¹ß»ç¹æÇâ
                MonsterRenderer.flipX = false;//½ºÇÁ¶óÀÌÆ® ¹İÀüx(¿À¸¥ÂÊ º½)
            }
            else if((player.transform.position.x - gameObject.transform.position.x < 0))
            {
                Dir = -1;//¹æÇâ ¿ŞÂÊ
                dirVec = new Vector3(-1, 0f, 0f);//Ray ¹ß»ç¹æÇâ
                MonsterRenderer.flipX = true;//½ºÇÁ¶óÀÌÆ® ¹İÀü O(¿ŞÂÊº¸°Ô)
            }

            if (Dist <= 1.3f)
            {
                Dir = 0;
            }
            Vector3 NextPos = new Vector3(Dir, 0, 0) * MaxSpeed * Time.deltaTime;
            transform.position = curPos + NextPos;//ÇÃ·¹ÀÌ¾î ÇâÇØ¼­ ÀÌµ¿(ÃßÀû)
        }
    }//±ú¾îÀÖ´Â µ¿¾È Çàµ¿ÇÏ´Â ·çÆ¾

    IEnumerator CheckStop()
    {
        lastPlayerPosition = Player.transform.position;

        yield return new WaitForSeconds(0.1f);
        //Debug.Log("°Ë»çÇÕ´Ï´Ù.");
        if (lastPlayerPosition == Player.transform.position)
        {
            //Debug.Log("¸ØÃè½À´Ï´Ù.");
            moveTimeOne = 0;//ÇÃ·¹ÀÌ¾î°¡ ¿òÁ÷ÀÎ ½Ã°£ ÃÊ±âÈ­
            //moveTimeTwo = 0;//ÇÃ·¹ÀÌ¾î°¡ ¿òÁ÷ÀÎ ½Ã°£ ÃÊ±âÈ­
            isPlayerStop = true;//ÇÃ·¹ÀÌ¾î°¡ ¸ØÃè½À´Ï´Ù.
        }//¸Ø­ŸÀ¸¹Ç·Î °Ë»çÇÏ´ø ½Ã°£µé ÃÊ±âÈ­
        else
        {
            //Debug.Log("°è¼Ó ¿òÁ÷ÀÌ´Â ÁßÀÔ´Ï´Ù.");
        }//°è¼Ó ¿òÁ÷ÀÓÀ» È®ÀÎ
    }//¸ØÃè´ÂÁö¸¦ È®ÀÎÇÔ 0.4ÃÊ°£ ¿òÁ÷ÀÌÁö ¾Ê¾Æ¾ß ¸ØÃá°É·Î ÀÓ½ÃÆÇÁ¤ 
    
    public void AreaCheck(int state)
    {
        switch (state)
        {
            case 0://±íÀº¼ö¸é »óÅÂÀÏ¶§ °Ë»ç
                if(Dist <= 20)
                {
                    SleepState = 1;//Áß°£¼ö¸éÀ¸·Î
                    animator.SetInteger("SleepState", 1);
                }
                break;
            case 1://±íÀº¼ö¸é »óÅÂÀÏ¶§ °Ë»ç
                if (Dist <= 20)
                {
                    if (moveTimeOne >= 0.8f)//0.8ÃÊ ÀÌ»ó ¿òÁ÷¿´´Ù¸é
                    {
                        MonsterAwake();
                    }//°Å¸®°¡ 30¾È¿¡ µé¾î¿À¸é Ã¼Å©(±íÀº¼ö¸é»óÅÂ)

                    CheckPlayerDash();//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬ÇßÀ½À» È®ÀÎÇÔ.
                }
                else if (Dist >= 20)
                {
                    SleepState = 0;//±íÀº ¼ö¸éÀ¸·Î µ¹¾Æ°¨
                    moveTimeOne = 0;
                    animator.SetInteger("SleepState", 0);
                }
                break;
        }
    }

    public void CheckPlayerDash()
    {
        if (player.GetDashCheck() == true)//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬¸¦ Çß´Ù¸é
        {
            MonsterAwake();//±â»ó
        }
    }//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬Çß´ÂÁö Ã¼Å©ÇÔ

    public void MonsterAwake()
    {
        SleepState = 3;//±ú¾î³­ »óÅÂ
        animator.SetInteger("SleepState", 3);//¾Ö´Ï¸ŞÀÌ¼ÇÀ» ±â»ó »óÅÂ·Î º¯°æ
        //MonsterRenderer.color = new Color(1, 0f, 0f, 1f);
        animator.SetBool("isMove", true);//ÇÃ·¹ÀÌ¾î¸¦ ÃßÀûÇØ¿À±â ¶§¹®¿¡ isMove ÆÄ¶ó¹ÌÅÍ°ªÀ» true·Î º¯°æ
        isMobMove = true;//¸ó½ºÅÍ´Â ¿òÁ÷ÀÎ´Ù.
        //Debug.Log("±ú¾î³µ½À´Ï´Ù.");
    }

    public override void damaged(float damage)
    {
        Health -= damage;//Ã¼·Â¿¡¼­ µ¥¹ÌÁö¸¸Å­ ±ğÀ½
        //Debug.Log("¸ó½ºÅÍ°¡ µ¥¹ÌÁö¸¦ ÀÔ¾ú½À´Ï´Ù.");
        //¸ğ¼Ç
        //»ç¿îµå

        if(SleepState != 3)
        {
            MonsterAwake();//¼ö¸éÁßÀÏ¶§ °ø°İ´çÇÏ¸é ±â»ó
        }

        if (Health <= 0 && !isDead)
        {
            Die();//»ç¸Á
        }//Ã¼·ÂÀÌ 0ÀÌ µÇ°Å³ª 0¾Æ·¡·Î ¶³¾îÁ³°í Á×Áö ¾Ê¾Ò´Ù¸é
        //ÇÇ°İ½Ã ½ÇÇàÇÒ ³»¿ë
    }

    public override void Die()
    {
        isMobMove = false;//¿òÁ÷ÀÓ ¸ØÃã
        isDead = true;//»ç¸ÁÇßÀ½
        animator.SetTrigger("Die");//¾Ö´Ï¸ŞÀÌÅÍ¿¡ Die Æ®¸®°Å¸¦ Àü´ŞÇØ¼­ »ç¸Á ¾Ö´Ï¸ŞÀÌ¼Ç Àç»ı
        Invoke("Destroy", 1.2f);//Àá½ÃÈÄ¿¡ ¿ÀºêÁ§Æ® ºñÈ°¼ºÈ­
    }

    private void Destroy()
    {
        gameObject.SetActive(false);//¿ÀºêÁ§Æ® ºñÈ°¼ºÈ­
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Dir = 0;
            isWall = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Wall")
        {
            Dir = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            isWall = false;
        }
    }
}
/*ÇÃ·¹ÀÌ¾î¿Í °Å¸®°¡ 30ÀÌÇÏ°¡ µÈ´Ù  -> °Ë»ç½ÃÀÛ -> 2ÃÊ°£ °ÉÀ¸¸é Áß°£¼ö¸é
				          -> ´ë½¬ÇÏ¸é ¹Ù·Î ±â»ó
Áß°£¼ö¸é¿¡¼­ 15°Å¸®±îÁö µé¾î¿Ô°í ¾È¿¡¼­ 0.8ÃÊ°£ °É¾ú´Â°¡? ->  ¾èÀº ¼ö¸é -> ¿òÁ÷ÀÌÀÚ¸¶ÀÚ ±â»ó
->  ¾Æ´Ï¸é ´Ù½Ã  ±íÀº ¼ö¸éÀ¸·Î*/
//ÃÊ->³ë->»¡->»¡(À¯Áö)


//case 2://Áß°£¼ö¸é »óÅÂÀÏ¶§ °Ë»ç
//    if (SleepState == 1 && Dist <= 15)//Áß°£¼ö¸é »óÅÂÀÌ°í, ÇÃ·¹ÀÌ¾î°¡ 15ÀÇ ¹üÀ§³»¿¡ ÀÖÀ»¶§
//    {
//        if (moveTimeTwo >= 0.8)//0.8ÃÊ ÀÌ»ó °ÉÀ¸¸é
//        {
//            SleepState = 2;//¾èÀº ¼ö¸é »óÅÂ·Î µé¾î°¨
//            moveTimeTwo = 0;//2Â÷ °Ë»ç½Ã°£ ÃÊ±âÈ­
//            animator.SetInteger("SleepState", 2);//¾èÀº ¼ö¸é ¾Ö´Ï¸ŞÀÌ¼ÇÀ¸·Î º¯°æ
//            //Debug.Log("¾èÀº¼ö¸é »óÅÂ·Î µé¾î°©´Ï´Ù.");
//        }
//        else if(isPlayerStop)//¸¸¾à ¸ØÃè´Ù¸é
//        {
//            SleepState = 0;//´Ù½Ã ±íÀº ¼ö¸é »óÅÂ·Î µé¾î°¨
//            //MonsterRenderer.color = new Color(1, 1, 1, 1);
//            animator.SetInteger("SleepState", 0);//±íÀº ¼ö¸é »óÅÂ·Î ¾Ö´Ï¸ŞÀÌ¼Ç ÀüÈ¯
//            //Debug.Log("±íÀº ¼ö¸é »óÅÂ·Î µ¹¾Æ°©´Ï´Ù.");
//        }
//    }//Áß°£¼ö¸é »óÅÂÀÌ°í 15¾È¿¡¼­ 0.8ÃÊ°£ ¿òÁ÷¿´³ª?
//    break;
//case 2://¾èÀº ¼ö¸é »óÅÂÀÏ¶§ °Ë»ç
//    if (SleepState == 2)//¾èÀº ¼ö¸é»óÅÂÀÏ¶§
//    {
//        if (player.GetMoveCheck())//¿òÁ÷¿´´Ù¸é
//        {
//            MonsterAwake();//±â»ó
//        }
//        else//¿òÁ÷ÀÌÁö ¾Ê¾Ò´Ù¸é
//        {
//            SleepState = 1;//Áß°£ ¼ö¸é »óÅÂ·Î µ¹¾Æ°¨
//            //MonsterRenderer.color = new Color(1, 0.92f, 0.016f, 1);
//            animator.SetInteger("SleepState", 1);//Áß°£ ¼ö¸é »óÅÂ·Î ¾Ö´Ï¸ŞÀÌ¼Ç º¯°æ
//            //Debug.Log("Áß°£ ¼ö¸é »óÅÂ·Î µ¹¾Æ°©´Ï´Ù.");
//        }
//    }//¾èÀº ¼ö¸é »óÅÂ ÀÌ »óÅÂ¿¡ ÇÃ·¹ÀÌ¾î°¡ °ÉÀ¸¸é ¹Ù·Î ±â»ó