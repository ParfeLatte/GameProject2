using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    //public GameObject small;//¹üÀ§ 15ÀÇ Äİ¶óÀÌ´õ ¿µ¿ª
    //public GameObject Big;//¹üÀ§ 30ÀÇ Äİ¶óÀÌ´õ ¿µ¿ª
    public GameObject Player;//°Å¸®ÃøÁ¤¿ë

    public int SleepState;//ÇöÀç ¼ö¸é»óÅÂ 0:±íÀº¼ö¸é, 1:Áß°£¼ö¸é, 2:¾èÀº¼ö¸é, 3:±â»ó!!!

    public float moveTimeOne;//ÇÃ·¹ÀÌ¾î°¡ ¾ó¸¶³ª °É¾ú´ÂÁö È®ÀÎ
    public float moveTimeTwo;//ÇÃ·¹ÀÌ¾î°¡ ¾ó¸¶³ª °É¾ú´ÂÁö È®ÀÎ
    public float CheckTime;//¹üÀ§ ¾È¿¡ µé¾î¿Â ½Ã°£
    public float Dist;//¸ó½ºÅÍ¿Í ÇÃ·¹ÀÌ¾î »çÀÌ °Å¸®
    public float Dir;//ÀÌµ¿¹æÇâ
    
    
    public Vector3 lastPlayerPosition;//

    private Rigidbody2D MR;
    private Animator animator;
    private SpriteRenderer MonsterRenderer;

    public Player player;//ÇÃ·¹ÀÌ¾î ÄÚµå

    public bool isPlayerDash;//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬Çß´ÂÁö È®ÀÎ
    public bool isPlayerStop;//ÇÃ·¹ÀÌ¾î°¡ ¸ØÃè´Â°¡?

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MonsterRenderer = GetComponent<SpriteRenderer>();
        SleepState = 0;
        SetStatus(100, 100, 4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//ÇÃ·¹ÀÌ¾î¿Í ¸ó½ºÅÍ »çÀÌ °Å¸®
        Vector3 curPos = transform.position;//ÇöÀçÀ§Ä¡
        

        if (Dist <= 30 && SleepState != 3)
        {
            if(SleepState == 0)
            {
                moveTimeOne += Time.deltaTime;
            }//±íÀº ¼ö¸éÀÏ¶§ °Ë»çÇÏ´Â ½Ã°£
            else if(SleepState  == 1 && Dist <= 15)
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

        if(SleepState == 3)
        {
            if(player.transform.position.x - gameObject.transform.position.x >= 0)
            {
                Dir = 1;
                MonsterRenderer.flipX = false;
            }
            else
            {
                Dir = -1;
                MonsterRenderer.flipX = true;
            }


            Vector3 NextPos = new Vector3(Dir, 0, 0) * MaxSpeed * Time.deltaTime;
            transform.position = curPos + NextPos;
        }
       
    }

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
                        SleepState = 3;
                        animator.SetInteger("SleepState", 3);
                        Debug.Log("±ú¾î³µ½À´Ï´Ù.");
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
            SleepState = 3;
            animator.SetInteger("SleepState", 3);
            Debug.Log("±ú¾î³µ½À´Ï´Ù.");
        }
    }//ÇÃ·¹ÀÌ¾î°¡ ´ë½¬Çß´ÂÁö Ã¼Å©ÇÔ
}
/*ÇÃ·¹ÀÌ¾î¿Í °Å¸®°¡ 30ÀÌÇÏ°¡ µÈ´Ù  -> °Ë»ç½ÃÀÛ -> 2ÃÊ°£ °ÉÀ¸¸é Áß°£¼ö¸é
				          -> ´ë½¬ÇÏ¸é ¹Ù·Î ±â»ó
Áß°£¼ö¸é¿¡¼­ 15°Å¸®±îÁö µé¾î¿Ô°í ¾È¿¡¼­ 0.8ÃÊ°£ °É¾ú´Â°¡? ->  ¾èÀº ¼ö¸é -> ¿òÁ÷ÀÌÀÚ¸¶ÀÚ ±â»ó
->  ¾Æ´Ï¸é ´Ù½Ã  ±íÀº ¼ö¸éÀ¸·Î*/
//ÃÊ->³ë->»¡->»¡(À¯Áö)