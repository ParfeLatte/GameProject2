using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    //public GameObject small;//범위 15의 콜라이더 영역
    //public GameObject Big;//범위 30의 콜라이더 영역
    public GameObject Player;//거리측정용
    public Transform AttackPos;//공격범위 위치
    public Vector2 boxSize;//공격범위

    public int SleepState;//현재 수면상태 0:깊은수면, 1:중간수면, 2:얕은수면, 3:기상!!!

    public float moveTimeOne;//플레이어가 얼마나 걸었는지 확인
    public float moveTimeTwo;//플레이어가 얼마나 걸었는지 확인
    public float CheckTime;//범위 안에 들어온 시간
    public float CoolTime;//공격 쿨타임
    public float AttackTime;//공격시간
    public float Dist;//몬스터와 플레이어 사이 거리
    public float Dir;//이동방향
    public Vector3 dirVec;//레이 방향

    public Player player;//플레이어 코드
    //public MonsterAttack Attack;//공격 실행 스크립트

    public bool isPlayerDash;//플레이어가 대쉬했는지 확인
    public bool isPlayerStop;//플레이어가 멈췄는가?

    public Vector3 lastPlayerPosition;//위치 비교용


    private Rigidbody2D MR;
    private Animator animator;
    private SpriteRenderer MonsterRenderer;

    private Vector3 curPos;//현재위치
    private Vector2 AddPos = new Vector2(0, 3f);//pivot을 아래로 고정했으므로 레이 검사때 위로
    private Vector2 RayPos;
    private GameObject AttackPlayer;//

    private bool isMobMove = false;//몬스터가 움직이는지

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MonsterRenderer = GetComponent<SpriteRenderer>();
        SleepState = 0;
        SetStatus(31, 10, 4.5f);//일단 일반몹기준
        Health = MaxHealth;//체력을 최대체력으로 설정 
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//플레이어와 몬스터 사이 거리
        curPos = transform.position;//현재위치
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
            Debug.Log("플레이어가 사정거리내에 있음");
            if (AttackPlayer != null && AttackTime >= CoolTime)
            {
                isMobMove = false;
                animator.SetBool("isMove", false);
                animator.SetTrigger("Attack");
                Invoke("Attack", 0.15f);
                AttackTime = 0;
                Invoke("MoveAgain", 0.15f); ;
            }
            Debug.Log("공격합니다.");
        }
        else
        {
            AttackPlayer = null;
            Debug.Log("사정거리내에 아무도 없음");
            Debug.Log("계속 추적합니다");
        }
    }//공격

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
    }//범위내 공격
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
            }//깊은 수면일때 검사하는 시간
            else if (SleepState == 1 && Dist <= 15)
            {
                moveTimeTwo += Time.deltaTime;
            }//중간 수면일때 검사하는 시간

            if (!player.isMove)
            {
                StartCoroutine("CheckStop");
            }//멈췄으면 멈춤검사
            else
            {
                isPlayerStop = false;
            }//멈추지 않았음
            AreaCheck(SleepState);//수면검사!
        }
    }
    private void OnWake()
    {
        Debug.DrawRay(RayPos, dirVec * 2f, new Color(0, 1, 0));//레이캐스트 표시(거리 확인용)
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
    }//깨어있는 동안 행동하는 루틴

    IEnumerator CheckStop()
    {
        lastPlayerPosition = Player.transform.position;

        yield return new WaitForSeconds(0.1f);
        //Debug.Log("검사합니다.");
        if (lastPlayerPosition == Player.transform.position)
        {
            //Debug.Log("멈췄습니다.");
            moveTimeOne = 0;
            moveTimeTwo = 0;
            isPlayerStop = true;
        }//멈췃으므로 검사하던 시간들 초기화
        else
        {
            //Debug.Log("계속 움직이는 중입니다.");
        }//계속 움직임을 확인
    }//멈췄는지를 확인함 0.4초간 움직이지 않아야 멈춘걸로 임시판정 
    
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
                        Debug.Log("중간수면 상태로 들어갑니다.");
                    }
                }//거리가 30안에 들어오면 체크(깊은수면상태)
                break;
            case 1:
                if (SleepState == 1 && Dist <= 15)
                {
                    if (moveTimeTwo >= 0.8)
                    {
                        SleepState = 2;
                        animator.SetInteger("SleepState", 2);
                        Debug.Log("얕은수면 상태로 들어갑니다.");
                    }
                    else if(isPlayerStop)
                    {
                        SleepState = 0;
                        animator.SetInteger("SleepState", 0);
                        Debug.Log("깊은 수면 상태로 돌아갑니다.");
                    }
                }//중간수면 상태이고 15안에서 0.8초간 움직였나?
                break;
            case 2:
                if (SleepState == 2)
                {
                    if (player.GetMoveCheck())
                    {
                        MonsterAwake();//기상
                    }
                    else
                    {
                        SleepState = 1;
                        animator.SetInteger("SleepState", 1);
                        Debug.Log("중간 수면 상태로 돌아갑니다.");
                    }
                }//얕은 수면 상태 이 상태에 플레이어가 걸으면 바로 기상
                break;
        }
        CheckPlayerDash();
    }
    public void CheckPlayerDash()
    {
        if (player.GetDashCheck() == true)
        {
            MonsterAwake();//기상
        }
    }//플레이어가 대쉬했는지 체크함

    private void MonsterAwake()
    {
        SleepState = 3;
        animator.SetInteger("SleepState", 3);
        animator.SetBool("isMove", true);
        isMobMove = true;
        Debug.Log("깨어났습니다.");
    }

    public override void damaged(float damage)
    {
        Health -= damage;//체력에서 데미지만큼 깎음
        Debug.Log("몬스터 아야함");
        //모션
        //사운드

        if (Health <= 0 && !isDead)
        {
            Die();
        }//체력이 0이 되거나 0아래로 떨어졌고 죽지 않았다면
        //피격시 실행할 내용
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
/*플레이어와 거리가 30이하가 된다  -> 검사시작 -> 2초간 걸으면 중간수면
				          -> 대쉬하면 바로 기상
중간수면에서 15거리까지 들어왔고 안에서 0.8초간 걸었는가? ->  얕은 수면 -> 움직이자마자 기상
->  아니면 다시  깊은 수면으로*/
//초->노->빨->빨(유지)

//RaycastHit2D rayHit = Physics2D.Raycast(MR.position, dirVec, 1.2f, LayerMask.GetMask("Player"));
//if (rayHit.collider != null)
//{
//    LivingEntity player = rayHit.collider.GetComponent<Player>();
//    AttackPlayer = rayHit.collider.gameObject;
//    Debug.Log("플레이어가 사정거리내에 있음");
//    if (AttackPlayer != null && !isAttack)
//    {
//        isAttack = true;
//        player.damaged(damage);
//        isAttack = false;
//    }
//    Debug.Log("공격합니다.");
//}
//else
//{
//    AttackPlayer = null;
//    Debug.Log("사정거리내에 아무도 없음");
//    Debug.Log("계속 추적합니다");
//}