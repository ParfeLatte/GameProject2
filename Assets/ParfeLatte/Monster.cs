using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    //public GameObject small;//범위 15의 콜라이더 영역
    //public GameObject Big;//범위 30의 콜라이더 영역
    public GameObject Player;//거리측정용
    public Transform AttackPos;//공격범위 위치
    public EventMonster Eventmob;//이벤트몹
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
    private Vector2 RayPos;//레이 방향
    private GameObject AttackPlayer;//공격대상

    private bool isMobMove = false;//몬스터가 움직이는지

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MonsterRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.Find("Player");
        SleepState = 0;//깊은수면 상태로 스폰
        SetStatus(30, 10, 4.5f);//일단 일반몹기준
        Health = MaxHealth;//체력을 최대체력으로 설정 
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//플레이어와 몬스터 사이 거리
        curPos = transform.position;//현재위치
        RayPos = MR.position + AddPos;//레이를 발사하는 위치
        if(SleepState != 3 & !isDead) { 
            StateCheck();//현재 수면상태이므로 수면상태 확인
        }//수면시 패턴
        else if(SleepState == 3 && !isDead) {
            OnWake();//깨어있을때 행동패턴
            AttackCheck();//공격범위 내에 들어오면 공격
        }//기상시 패턴
        AttackTime += Time.deltaTime;//쿨타임
    }

    private void AttackCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(RayPos, dirVec, 2f, LayerMask.GetMask("Player"));//Ray를 발사하여 플레이어가 범위내에 있는지 확인
        if (rayHit.collider != null)//레이에 충돌한 대상이 있으면
        {
            Player player = rayHit.collider.GetComponent<Player>();//플레이어 컴포넌트 할당
            AttackPlayer = rayHit.collider.gameObject;//공격할 플레이어 대상 할당
            //Debug.Log("플레이어가 사정거리내에 있음");
            if (AttackPlayer != null && AttackTime >= CoolTime)//공격 쿨타임이 돌았고, 플레이어가 할당되어있다면
            {
                isMobMove = false;//잠시 움직임을 멈추고
                animator.SetBool("isMove", false);//애니메이션 파라미터에서 isMove를 false로 바꾸고
                animator.SetTrigger("Attack");//attack Trigger를 발동해서 공격 애니메이션 재생
                Invoke("Attack", 0.15f);//애니메이션에서 휘두르는 모션에 맞게 공격
                AttackTime = 0;//다시 쿨타임
                Invoke("MoveAgain", 0.15f);//공격후에 다시 움직이도록
            }
            //Debug.Log("공격합니다.");
        }
        else
        {
            AttackPlayer = null;
            //Debug.Log("사정거리내에 아무도 없음");
            //Debug.Log("계속 추적합니다");
        }//예외처리
    }//공격

    private void Attack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(AttackPos.position, boxSize, 0);//오버랩 박스를 이용
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")//오버랩박스로 검사한 오브젝트 중에서 플레이어가 있으면
            {
                player.damaged(damage);//플레이어의 damaged 함수 호출해서 데미지를 줌
            }
        }
    }//범위내 공격

    public void MoveAgain()
    {
        isMobMove = true;//다시 움직임
        animator.SetBool("isMove", true);//이동 애니메이션 셋
    }

    private void StateCheck()
    {
        if (Dist <= 30 && SleepState != 3)
        {
            if (SleepState == 0)
            {
                moveTimeOne += Time.deltaTime;//30의 범위내에 있을때 검사하는 시간
            }//깊은 수면일때 검사하는 시간
            else if (SleepState == 1 && Dist <= 15)
            {
                moveTimeTwo += Time.deltaTime;//수면 상태가 중간 수면이고, 15의 범위내에 있을때 검사하는 시간
            }//중간 수면일때 검사하는 시간

            if (!player.isMove)
            {
                StartCoroutine("CheckStop");//플레이어의 움직임이 멈췄는지 검사
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
                Dir = 1;//방향 오른쪽
                dirVec = new Vector3(1f, 0f, 0f);//Ray 발사방향
                MonsterRenderer.flipX = false;//스프라이트 반전x(오른쪽 봄)
            }
            else
            {
                Dir = -1;//방향 왼쪽
                dirVec = new Vector3(-1, 0f, 0f);//Ray 발사방향
                MonsterRenderer.flipX = true;//스프라이트 반전 O(왼쪽보게)
            }
            Vector3 NextPos = new Vector3(Dir, 0, 0) * MaxSpeed * Time.deltaTime;
            transform.position = curPos + NextPos;//플레이어 향해서 이동(추적)
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
            moveTimeOne = 0;//플레이어가 움직인 시간 초기화
            moveTimeTwo = 0;//플레이어가 움직인 시간 초기화
            isPlayerStop = true;//플레이어가 멈췄습니다.
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
            case 0://깊은수면 상태일때 검사
                if (Dist <= 30)
                {
                    if (moveTimeOne >= 2.0)//2초 이상 움직였다면
                    {
                        SleepState = 1;//중간 수면 상태로 들어감
                        moveTimeOne = 0;//1차 검사시간은 초기화
                        animator.SetInteger("SleepState", 1);//애니메이션의 기상상태를 중간수면 상태로 바꿈
                        //Debug.Log("중간수면 상태로 들어갑니다.");
                    }
                }//거리가 30안에 들어오면 체크(깊은수면상태)
                break;
            case 1://중간수면 상태일때 검사
                if (SleepState == 1 && Dist <= 15)//중간수면 상태이고, 플레이어가 15의 범위내에 있을때
                {
                    if (moveTimeTwo >= 0.8)//0.8초 이상 걸으면
                    {
                        SleepState = 2;//얕은 수면 상태로 들어감
                        moveTimeTwo = 0;//2차 검사시간 초기화
                        animator.SetInteger("SleepState", 2);//얕은 수면 애니메이션으로 변경
                        //Debug.Log("얕은수면 상태로 들어갑니다.");
                    }
                    else if(isPlayerStop)//만약 멈췄다면
                    {
                        SleepState = 0;//다시 깊은 수면 상태로 들어감
                        animator.SetInteger("SleepState", 0);//깊은 수면 상태로 애니메이션 전환
                        //Debug.Log("깊은 수면 상태로 돌아갑니다.");
                    }
                }//중간수면 상태이고 15안에서 0.8초간 움직였나?
                break;
            case 2://얕은 수면 상태일때 검사
                if (SleepState == 2)//얕은 수면상태일때
                {
                    if (player.GetMoveCheck())//움직였다면
                    {
                        MonsterAwake();//기상
                    }
                    else//움직이지 않았다면
                    {
                        SleepState = 1;//중간 수면 상태로 돌아감
                        animator.SetInteger("SleepState", 1);//중간 수면 상태로 애니메이션 변경
                        //Debug.Log("중간 수면 상태로 돌아갑니다.");
                    }
                }//얕은 수면 상태 이 상태에 플레이어가 걸으면 바로 기상
                break;
        }
        CheckPlayerDash();//플레이어가 대쉬했음을 확인함.
    }

    public void CheckPlayerDash()
    {
        if (player.GetDashCheck() == true)//플레이어가 대쉬를 했다면
        {
            MonsterAwake();//기상
        }
    }//플레이어가 대쉬했는지 체크함

    public void MonsterAwake()
    {
        SleepState = 3;//깨어난 상태
        animator.SetInteger("SleepState", 3);//애니메이션을 기상 상태로 변경
        animator.SetBool("isMove", true);//플레이어를 추적해오기 때문에 isMove 파라미터값을 true로 변경
        isMobMove = true;//몬스터는 움직인다.
        //Debug.Log("깨어났습니다.");
    }

    public override void damaged(float damage)
    {
        Health -= damage;//체력에서 데미지만큼 깎음
        //Debug.Log("몬스터가 데미지를 입었습니다.");
        //모션
        //사운드

        if(SleepState != 3)
        {
            MonsterAwake();//수면중일때 공격당하면 기상
        }

        if (Health <= 0 && !isDead)
        {
            Die();//사망
        }//체력이 0이 되거나 0아래로 떨어졌고 죽지 않았다면
        //피격시 실행할 내용
    }

    public override void Die()
    {
        isMobMove = false;//움직임 멈춤
        isDead = true;//사망했음
        Eventmob.isDead = true;//이벤트몹 사망처리
        animator.SetTrigger("Die");//애니메이터에 Die 트리거를 전달해서 사망 애니메이션 재생
        Invoke("Destroy", 1.3f);//잠시후에 오브젝트 비활성화
    }

    private void Destroy()
    {
        gameObject.SetActive(false);//오브젝트 비활성화
    }
}
/*플레이어와 거리가 30이하가 된다  -> 검사시작 -> 2초간 걸으면 중간수면
				          -> 대쉬하면 바로 기상
중간수면에서 15거리까지 들어왔고 안에서 0.8초간 걸었는가? ->  얕은 수면 -> 움직이자마자 기상
->  아니면 다시  깊은 수면으로*/
//초->노->빨->빨(유지)