using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public float AttackTime;//공격한시간
    public float CoolTime;//쿨타임
    public float movetime;//이동한 시간 체크
    public float stoptime;//
    public float JumpForce;//점프력
    public float Dir;//이동방향(대쉬나 점프시에 방향 못바꾸게함)
    public float h;//GeTAxisRaw로 받는 값
    public float stamina;//스테미나
    public float dashtime;//대쉬한지 얼마나 지났는지 체크해서 스테미너 채움
    public float lastYpos;//점프나 땅에서 떨어졌을 때 마지막 y값
    public float CurYpos;//착지 시에 비교할 y값
    public float falltime;

    public bool isDash;//대쉬했는지 체크
    public bool isMove;//움직였는지 체크
    public bool isWall;//벽에 부딪혔는지 체크
    public bool isJump;//점프했는지 체크
    public bool isCharge;//공격충전중인지
    public bool isFallDamage;//낙뎀받았는지

    public Slider HealthSlider;//체력UI
    public Slider StaminaSlider;//스테미너UI
    

    //public PlayerAttack Attack;
    public PlayerAttack Attack;//공격을 실행해주는 스크립트(공격 범위 오브젝트에 할당되어 있으며 여기서 공격을 실행함)
    
    private GameObject Enemy;//몬스터


    private Rigidbody2D PR;//플레이어 리지드바디
    private SpriteRenderer PlayerRenderer;//스프라이트 좌우 바꿀때 사용했음
    private Animator animator;//애니메이터

    private float ChargeTime;//공격 차징시간
    private Vector3 curPos;//현재 위치
    private Vector3 dirVec;//바라보는 방향


    void Awake()
    {
        PR = GetComponent<Rigidbody2D>();//할당
        animator = GetComponent<Animator>();//할당
        PlayerRenderer = GetComponent<SpriteRenderer>();//할당
        SetStatus(100, 10, 8);//스탯을 설정함 체력, 데미지, 이동속도
        Health = MaxHealth;//시작할때 현재 체력을 최대 체력으로 설정해줌
        gameObject.SetActive(true);
        stamina = 100f;
        HealthSlider.value = Health;
        StaminaSlider.value = stamina;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * 1.75f, new Color(0, 1, 0));
        h = Input.GetAxisRaw("Horizontal");
        curPos = transform.position;//현재위치
        if(h != 0 && !isDash && !isJump)
        {
            Dir = h;
            dirVec = new Vector3(h, 0f, 0f);
            isMove = true;
            animator.SetBool("isMove", true);
        }
        else if(h == 0)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }//움직이는지 확인함

        AttackTime += Time.deltaTime;
        
        flipSpr();//좌우반전
        if (!isDead)
        {
            AttackCheck();//공격
            Jump();//점프
            Dash();//대쉬
            Move();//이동
            StaminaCheck();
            CheckFallDamage();
            dashtime += Time.deltaTime;
        }
    }

    private void AttackCheck()
    {
            if (Input.GetKeyDown(KeyCode.Z) && AttackTime >= CoolTime && !isCharge)
            {
                animator.SetBool("isCharge", true);
            }
            if (Input.GetKey(KeyCode.Z) && AttackTime >= CoolTime)//키는 임시임 J를 누르는 중일때
            {
                ChargeTime += Time.deltaTime;
                h = 0;

                isCharge = true;
                //Debug.Log("공격 차징중");
            }
            if (Input.GetKeyUp(KeyCode.Z) && isCharge && AttackTime >= CoolTime)//키를 뗐을때
            {
                isCharge = false;
                animator.SetBool("isCharge", false);
                if (ChargeTime < 1f)//차징 시간이 1초 이하이면 기본공격
                {
                    //Debug.Log("기본 공격");
                    Attack.GetAttack(damage);//PlayerAttack 스크립트에 데미지를 전달해주고 PlayerAttack에서는 공격을 실행함
                    animator.SetTrigger("Attack");//공격 애니메이션 재생
                    AttackTime = 0;
                }
                else if (ChargeTime >= 1.0f)//차징 시간이 1초 이상이면 강공
                {
                    //Debug.Log("강화 공격");
                    Attack.GetAttack(damage * 3.0f);//위와 같으나 3배의 데미지를 가함
                    animator.SetTrigger("Attack");//공격 애니메이션 재생
                    AttackTime = 0;
                    //강공 모션
                }
                ChargeTime = 0;//차징 타임 초기화
            }
    }//공격 체크, 공격버튼을 누르고 있으면 차징함!

    private void flipSpr()
    {
        if (!isDash && !isJump) {
            if (h == 1)
            {
                PlayerRenderer.flipX = false;//오른쪽을 바라보도록
                
            }
            else if (h == -1)
            {
                PlayerRenderer.flipX = true;//왼쪽을 바라보도록
            }
        }
    }//방향에 맞게 스프라이트 뒤집음
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJump)
        {
            isJump = true;//점프했다
            PR.velocity = Vector2.up * JumpForce;//순간 속력을 위로 점프력만큼 줌
            SetDirection(Dir);//점프시 이동가능한 방향(반대로 방향 조절 못하게)
            animator.SetBool("isJump", true);
            //Invoke("JumpReset", 0.8f);//1초 뒤에 점프리셋(수정할수도있음)
            //Debug.Log("스페이스바 눌림");
        }
        else if (isJump)
        {
            if (h == Dir)
            {
                h = Dir;//점프시 이동가능한 방향
            }
            else
            {
                h = 0;//점프시 방향 전환을 못하게끔
            }
        }
    }//점프를 처리하는 함수

    private void Move()
    {
        if (!isDash || !isWall || !isCharge)
        {
            Vector2 newVel = new Vector2(h * MaxSpeed, PR.velocity.y);//플레이어 이동속도
            PR.velocity = newVel;//리지드바디에 속도 등록
        }//대쉬중이 아닐때 이동속도를 이걸로 정해줌
    }//움직임을 처리하는 함수

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash && stamina >= 25f)
        {
            isDash = true;//대쉬했다를 체크
            animator.SetBool("isDash", true);
            stamina -= 25f;
            dashtime = 0f;
            Invoke("DashReset", 0.25f);
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 2 * MaxSpeed * Time.deltaTime;//대쉬할때 다음위치
            transform.position = curPos + DashPos;//더해서 위치변경
        }
    }//대쉬 함수
    
    private void StaminaCheck()
    {
        if(dashtime >= 1.25f && stamina <= 100f)
        {
            stamina += 5 * Time.deltaTime;
            if(stamina >= 100f)
            {
                stamina = 100f;
            }
        }
        StaminaSlider.value = stamina;
    }//스테미나 회복검사 

    public void SetDirection(float h) 
    {
        Dir = h;//마지막 방향 저장(체크할때 사용)
    }

    public bool GetMoveCheck()
    {
        return isMove;
    }

    public bool GetDashCheck()
    {
        return isDash;
    }//대쉬했음을 체크해서 보내줌

    private void DashReset()
    {
        isDash = false;
        animator.SetBool("isDash", false);
    }//대쉬이후 초기화

    //private void JumpReset()
    //{
    //    isJump = false;
    //}//점프 이후 초기화

    public override void Die()
    {

        base.Die();
        animator.SetTrigger("Die");//애니메이터에 Die 트리거를 전달해서 사망 애니메이션 재생
        Invoke("Dead", 1.2f);//잠시후에 오브젝트 비활성화
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        HealthCheck();
    }

    public override void damaged(float damage)
    {
        base.damaged(damage);
        Debug.Log(damage);
        HealthCheck();
    }

    private void Dead()
    {
        gameObject.SetActive(false);//오브젝트 비활성화
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HealthCheck()
    {
        HealthSlider.value = Health;
    }

    //private void SetLastPos()
    //{
    //    lastYpos = transform.position.y;
    //    isFallDamage = false;
    //    Debug.Log("떨어짐");
    //}

    private void FallDamage(float time)
    {
       if(time >= 1.5f)
        {
            damaged(70f);
        }
        else
        {
            damaged(35f);
        }
    }
    private void CheckFallDamage()
    {
        RaycastHit2D FloorRay = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Floor"));
        if(FloorRay.collider == null)
        {
            falltime += Time.deltaTime;
        }
        else if(FloorRay.collider != null)
        {
            if(falltime > 1.0f)
            {
                FallDamage(falltime);
            }
            falltime = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
            animator.SetBool("isJump", false);
            //if (!isFallDamage)
            //{
            //    CheckFallDamage();  
            //}
        }
        if (col.gameObject.tag == "Wall")
        {
            h = 0;
            Dir = 0;
            isWall = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Dir = 0;
            h = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            isWall = false;
        }
        //if(col.gameObject.tag == "Floor")
        //{
        //    SetLastPos();
        //}
    }
}
//Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//키입력에 따른 다음 위치
//transform.position = curPos + nextPos;//현재위치와 다음위치를 더함으로써 이동(이전 로직 지금은 사용안함)