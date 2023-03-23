using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    //public float Speed;//최대 이동속도
    //public float movetime;//이동한 시간 체크
    //public float stoptime;
    public float Dir = 1;
    public float LastDir = 1; //점프시 재이동가능한 방향
    public float JumpForce;//점프 높이
    public float JumpTime;//점프시간
    public float MaxJump;//최대점프높이
    public float h;//이동방향

    public bool isDash;//대쉬했는지 체크
    public bool isMove;//움직였는지 체크
    public bool isJump;//점프중인지 체크

    Rigidbody2D PlayerRigid;

    void Awake()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();
        SetStatus(100, 100, 4);
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");//이동방향을 받아옴

        if (h != 0 && !isDash && !isJump)
        {
            Dir = h;//마지막으로 움직였던 방향 저장
            isMove = true;
        }//움직이는 상태
        else if (h == 0)
        {
            isMove = false;
        }
        Dash();//대쉬
        Jump();//점프
        Move();//이동(좌우)
    }

    public void Move()
    {
        if (!isDash)
        {
            Vector2 newVelocity = new(h * MoveSpeed, PlayerRigid.velocity.y);//이동속도(방향 x 최고속도)
            PlayerRigid.velocity = newVelocity;//이동을 부드럽게 하기 위해서 속도를 고정
        }
    }//이동

    private void Dash()
    {
        //대쉬
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDash == false)
        {
            isDash = true;//대쉬했다를 체크
            Invoke("DashReset", 0.3f);//대쉬 시간을 설정가능함 현재 0.3초
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 3 * MoveSpeed * Time.deltaTime;
            transform.position += DashPos;
            isMove = true;
        }//대쉬키 누르면 isDash가 켜지고 그동안 대쉬함(처음에 정해진 방향으로 대쉬, 기본은 오른쪽방향인 1)
        //대쉬할때는 다른 행동 못함
    }//대쉬

    private void Jump()
    {
        //점프
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            SaveLastDir(Dir);//점프시 이동가능한 방향 저장(마지막으로 움직인 방향)
            PlayerRigid.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);//위로 힘을 줌
            Debug.Log("스페이스바 눌림");
            Invoke("JumpReset", JumpTime);//수정예정
        }

        if (isJump)
        {
            isMove = true;

            if (h == 0 || h != LastDir)
            {
                h = 0;
            }
            else
            {
                h = LastDir;
            }
        }//점프중일때 방향 전환 막음 + 멈췄다 앞으로 나아가는건 가능
    }//점프를 담당

    public bool GetMoveCheck()
    {
        return isMove;
    }

    public bool GetDashCheck()
    {
        return isDash;
    }//대쉬했음을 체크해서 보내줌

    private void SaveLastDir(float Dir) {
        LastDir = Dir;
    }//마지막 방향을 저장

    private void DashReset()
    {
        isDash = false;
    }//대쉬이후 초기화

    private void JumpReset()
    {
        isJump = false;
    }//점프 초기화(이건 나중에 또 따로 충돌 이벤트로 바꿀거임)

    void OnCollisionEnter()
    {

    }
}
