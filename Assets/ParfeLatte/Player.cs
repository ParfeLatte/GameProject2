using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{ 
    public float movetime;//이동한 시간 체크
    public float stoptime;
    public float JumpForce;//점프력
    public float Dir;//이동방향(대쉬나 점프시에 방향 못바꾸게함)
    public float h;//GeTAxisRaw로 받는 값

    public bool isDash;//대쉬했는지 체크
    public bool isMove;//움직였는지 체크
    public bool isJump;//점프했는지 체크

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
        SetStatus(100, 15, 8);//스탯을 설정함
        Health = MaxHealth;//시작할때 현재 체력을 최대 체력으로 설정해줌
    }

    // Update is called once per frame
    void Update()
    {
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
        
        flipSpr();//좌우반전
        Jump();//점프
        Dash();//대쉬
        Move();//이동
        AttackCheck();//공격
    }

    private void AttackCheck()
    {
        if (Input.GetKey(KeyCode.J))//키는 임시임 J를 누르는 중일때
        {
            ChargeTime += Time.deltaTime;
            //차징 애니메이숀
            //Debug.Log("공격 차징중");
        }
        if (Input.GetKeyUp(KeyCode.J))//키를 뗐을때
        {
            if (ChargeTime <= 0.5f)//차징 시간이 0.5초 이하이면 기본공격
            {
                //Debug.Log("기본 공격");
                Attack.GetAttack(damage);//PlayerAttack 스크립트에 데미지를 전달해주고 PlayerAttack에서는 공격을 실행함
                //기본공격 모션
            }
            else if (ChargeTime >= 1.0f)//차징 시간이 1초 이상이면 강공
            {
                //Debug.Log("강화 공격");
                Attack.GetAttack(damage * 2.0f);//위와 같으나 2배의 데미지를 가함
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
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;//점프했다
            PR.velocity = Vector2.up * JumpForce;//순간 속력을 위로 점프력만큼 줌
            SetDirection(Dir);//점프시 이동가능한 방향(반대로 방향 조절 못하게)
            Invoke("JumpReset", 1f);//1초 뒤에 점프리셋(수정할수도있음)
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
        if (!isDash)
        {
            Vector2 newVel = new Vector2(h * MaxSpeed, PR.velocity.y);//플레이어 이동속도
            PR.velocity = newVel;//리지드바디에 속도 등록
        }//대쉬중이 아닐때 이동속도를 이걸로 정해줌
    }//움직임을 처리하는 함수

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash)
        {
            isDash = true;//대쉬했다를 체크
            Invoke("DashReset", 0.4f);
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 2 * MaxSpeed * Time.deltaTime;//대쉬할때 다음위치
            transform.position = curPos + DashPos;//더해서 위치변경
        }
    }//대쉬 함수

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
    }//대쉬이후 초기화

    private void JumpReset()
    {
        isJump = false;
    }//점프 이후 초기화
}
//Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//키입력에 따른 다음 위치
//transform.position = curPos + nextPos;//현재위치와 다음위치를 더함으로써 이동(이전 로직 지금은 사용안함)