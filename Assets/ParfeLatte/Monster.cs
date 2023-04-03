using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    //public GameObject small;//¹üÀ§ 15ÀÇ ÄÝ¶óÀÌ´õ ¿µ¿ª
    //public GameObject Big;//¹üÀ§ 30ÀÇ ÄÝ¶óÀÌ´õ ¿µ¿ª
    public GameObject Player;//°Å¸®ÃøÁ¤¿ë
    public Transform AttackPos;//°ø°Ý¹üÀ§ À§Ä¡
    public Vector2 boxSize;//°ø°Ý¹üÀ§

    public int SleepState;//ÇöÀç ¼ö¸é»óÅÂ 0:±íÀº¼ö¸é, 1:Áß°£¼ö¸é, 2:¾èÀº¼ö¸é, 3:±â»ó!!!

    public float moveTimeOne;//ÇÃ·¹ÀÌ¾î°¡ ¾ó¸¶³ª °É¾ú´ÂÁö È®ÀÎ
    public float moveTimeTwo;//ÇÃ·¹ÀÌ¾î°¡ ¾ó¸¶³ª °É¾ú´ÂÁö È®ÀÎ
    public float CheckTime;//¹üÀ§ ¾È¿¡ µé¾î¿Â ½Ã°£
    public float CoolTime;//°ø°Ý ÄðÅ¸ÀÓ
    public float AttackTime;//°ø°Ý½Ã°£
    public float Dist;//¸ó½ºÅÍ¿Í ÇÃ·¹ÀÌ¾î »çÀÌ °Å¸®
    public float Dir;//ÀÌµ¿¹æÇâ
    public Vector3 dirVec;//·¹ÀÌ ¹æÇâ

    public Player player;//ÇÃ·¹ÀÌ¾î ÄÚµå
    //public MonsterAttack Attack;//°ø°Ý ½ÇÇà ½ºÅ©¸³Æ®

    public bool isPlayerDash;//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬Çß´ÂÁö È®ÀÎ
    public bool isPlayerStop;//ÇÃ·¹ÀÌ¾î°¡ ¸ØÃè´Â°¡?

    public Vector3 lastPlayerPosition;//À§Ä¡ ºñ±³¿ë


    private Rigidbody2D MR;
    private Animator animator;
    private SpriteRenderer MonsterRenderer;

    private Vector3 curPos;//ÇöÀçÀ§Ä¡
    private Vector2 AddPos = new Vector2(0, 3f);//pivotÀ» ¾Æ·¡·Î °íÁ¤ÇßÀ¸¹Ç·Î ·¹ÀÌ °Ë»ç¶§ À§·Î
    private Vector2 RayPos;
    private GameObject AttackPlayer;//

    private bool isMobMove = false;//¸ó½ºÅÍ°¡ ¿òÁ÷ÀÌ´ÂÁö

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MonsterRenderer = GetComponent<SpriteRenderer>();
        SleepState = 0;
        SetStatus(31, 10, 4.5f);//ÀÏ´Ü ÀÏ¹Ý¸÷±âÁØ
        Health = MaxHealth;//Ã¼·ÂÀ» ÃÖ´ëÃ¼·ÂÀ¸·Î ¼³Á¤ 
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//ÇÃ·¹ÀÌ¾î¿Í ¸ó½ºÅÍ »çÀÌ °Å¸®
        curPos = transform.position;//ÇöÀçÀ§Ä¡
        RayPos = MR.position + AddPos;
        if(SleepState != 3 & !isDead) { 
            StateCheck();
        }
        if(SleepState == 3 && !isDead) {
            OnWake();
            AttackCheck();
        }
        AttackTime += Time.deltaTime;
    }

    private void AttackCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(RayPos, dirVec, 2f, LayerMask.GetMask("Player"));
        if (rayHit.collider != null)
        {
            Player player = rayHit.collider.GetComponent<Player>();
            AttackPlayer = rayHit.collider.gameObject;
            Debug.Log("ÇÃ·¹ÀÌ¾î°¡ »çÁ¤°Å¸®³»¿¡ ÀÖÀ½");
            if (AttackPlayer != null && AttackTime >= CoolTime)
            {
                isMobMove = false;
                animator.SetBool("isMove", false);
                animator.SetTrigger("Attack");
                Invoke("Attack", 0.15f);
                AttackTime = 0;
                Invoke("MoveAgain", 0.15f); ;
            }
            Debug.Log("°ø°ÝÇÕ´Ï´Ù.");
        }
        else
        {
            AttackPlayer = null;
            Debug.Log("»çÁ¤°Å¸®³»¿¡ ¾Æ¹«µµ ¾øÀ½");
            Debug.Log("°è¼Ó ÃßÀûÇÕ´Ï´Ù");
        }
    }//°ø°Ý

    private void Attack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(AttackPos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                player.damaged(damage);
            }
        }
    }//¹üÀ§³» °ø°Ý
    public void MoveAgain()
    {
        isMobMove = true;
        animator.SetBool("isMove", true);
    }

    private void StateCheck()
    {
        if (Dist <= 30 && SleepState != 3)
        {
            if (SleepState == 0)
            {
                moveTimeOne += Time.deltaTime;
            }//±íÀº ¼ö¸éÀÏ¶§ °Ë»çÇÏ´Â ½Ã°£
            else if (SleepState == 1 && Dist <= 15)
            {
                moveTimeTwo += Time.deltaTime;
            }//Áß°£ ¼ö¸éÀÏ¶§ °Ë»çÇÏ´Â ½Ã°£

            if (!player.isMove)
            {
                StartCoroutine("CheckStop");
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
        Debug.DrawRay(RayPos, dirVec * 2f, new Color(0, 1, 0));//·¹ÀÌÄ³½ºÆ® Ç¥½Ã(°Å¸® È®ÀÎ¿ë)
        if (isMobMove == true)
        {
            if (player.transform.position.x - gameObject.transform.position.x >= 0)
            {
                Dir = 1;
                dirVec = new Vector3(1f, 0f, 0f);
                MonsterRenderer.flipX = false;
            }
            else
            {
                Dir = -1;
                dirVec = new Vector3(-1, 0f, 0f);
                MonsterRenderer.flipX = true;
            }
            Vector3 NextPos = new Vector3(Dir, 0, 0) * MaxSpeed * Time.deltaTime;
            transform.position = curPos + NextPos;
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
            moveTimeOne = 0;
            moveTimeTwo = 0;
            isPlayerStop = true;
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
            case 0:
                if (Dist <= 30)
                {
                    if (moveTimeOne >= 2.0)
                    {
                        SleepState = 1;
                        moveTimeOne = 0;
                        animator.SetInteger("SleepState", 1);
                        Debug.Log("Áß°£¼ö¸é »óÅÂ·Î µé¾î°©´Ï´Ù.");
                    }
                }//°Å¸®°¡ 30¾È¿¡ µé¾î¿À¸é Ã¼Å©(±íÀº¼ö¸é»óÅÂ)
                break;
            case 1:
                if (SleepState == 1 && Dist <= 15)
                {
                    if (moveTimeTwo >= 0.8)
                    {
                        SleepState = 2;
                        animator.SetInteger("SleepState", 2);
                        Debug.Log("¾èÀº¼ö¸é »óÅÂ·Î µé¾î°©´Ï´Ù.");
                    }
                    else if(isPlayerStop)
                    {
                        SleepState = 0;
                        animator.SetInteger("SleepState", 0);
                        Debug.Log("±íÀº ¼ö¸é »óÅÂ·Î µ¹¾Æ°©´Ï´Ù.");
                    }
                }//Áß°£¼ö¸é »óÅÂÀÌ°í 15¾È¿¡¼­ 0.8ÃÊ°£ ¿òÁ÷¿´³ª?
                break;
            case 2:
                if (SleepState == 2)
                {
                    if (player.GetMoveCheck())
                    {
                        MonsterAwake();//±â»ó
                    }
                    else
                    {
                        SleepState = 1;
                        animator.SetInteger("SleepState", 1);
                        Debug.Log("Áß°£ ¼ö¸é »óÅÂ·Î µ¹¾Æ°©´Ï´Ù.");
                    }
                }//¾èÀº ¼ö¸é »óÅÂ ÀÌ »óÅÂ¿¡ ÇÃ·¹ÀÌ¾î°¡ °ÉÀ¸¸é ¹Ù·Î ±â»ó
                break;
        }
        CheckPlayerDash();
    }
    public void CheckPlayerDash()
    {
        if (player.GetDashCheck() == true)
        {
            MonsterAwake();//±â»ó
        }
    }//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬Çß´ÂÁö Ã¼Å©ÇÔ

    private void MonsterAwake()
    {
        SleepState = 3;
        animator.SetInteger("SleepState", 3);
        animator.SetBool("isMove", true);
        isMobMove = true;
        Debug.Log("±ú¾î³µ½À´Ï´Ù.");
    }

    public override void damaged(float damage)
    {
        Health -= damage;//Ã¼·Â¿¡¼­ µ¥¹ÌÁö¸¸Å­ ±ðÀ½
        Debug.Log("¸ó½ºÅÍ ¾Æ¾ßÇÔ");
        //¸ð¼Ç
        //»ç¿îµå

        if (Health <= 0 && !isDead)
        {
            Die();
        }//Ã¼·ÂÀÌ 0ÀÌ µÇ°Å³ª 0¾Æ·¡·Î ¶³¾îÁ³°í Á×Áö ¾Ê¾Ò´Ù¸é
        //ÇÇ°Ý½Ã ½ÇÇàÇÒ ³»¿ë
    }

    public override void Die()
    {
        isMobMove = false;
        isDead = true;
        animator.SetTrigger("Die");
        Invoke("Destroy", 1.43f);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
/*ÇÃ·¹ÀÌ¾î¿Í °Å¸®°¡ 30ÀÌÇÏ°¡ µÈ´Ù  -> °Ë»ç½ÃÀÛ -> 2ÃÊ°£ °ÉÀ¸¸é Áß°£¼ö¸é
				          -> ´ë½¬ÇÏ¸é ¹Ù·Î ±â»ó
Áß°£¼ö¸é¿¡¼­ 15°Å¸®±îÁö µé¾î¿Ô°í ¾È¿¡¼­ 0.8ÃÊ°£ °É¾ú´Â°¡? ->  ¾èÀº ¼ö¸é -> ¿òÁ÷ÀÌÀÚ¸¶ÀÚ ±â»ó
->  ¾Æ´Ï¸é ´Ù½Ã  ±íÀº ¼ö¸éÀ¸·Î*/
//ÃÊ->³ë->»¡->»¡(À¯Áö)

//RaycastHit2D rayHit = Physics2D.Raycast(MR.position, dirVec, 1.2f, LayerMask.GetMask("Player"));
//if (rayHit.collider != null)
//{
//    LivingEntity player = rayHit.collider.GetComponent<Player>();
//    AttackPlayer = rayHit.collider.gameObject;
//    Debug.Log("ÇÃ·¹ÀÌ¾î°¡ »çÁ¤°Å¸®³»¿¡ ÀÖÀ½");
//    if (AttackPlayer != null && !isAttack)
//    {
//        isAttack = true;
//        player.damaged(damage);
//        isAttack = false;
//    }
//    Debug.Log("°ø°ÝÇÕ´Ï´Ù.");
//}
//else
//{
//    AttackPlayer = null;
//    Debug.Log("»çÁ¤°Å¸®³»¿¡ ¾Æ¹«µµ ¾øÀ½");
//    Debug.Log("°è¼Ó ÃßÀûÇÕ´Ï´Ù");
//}