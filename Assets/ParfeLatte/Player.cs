using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{ 
    public float movetime;//이동한 시간 체크
    public float stoptime;
    public float JumpForce;
    public float Dir;//이동방향(대쉬나 점프시에 방향 못바꾸게함)

    public bool isDash;//대쉬했는지 체크
    public bool isMove;//움직였는지 체크
    public bool isJump;//점프했는지 체크

    private Rigidbody2D PR;//플레이어 리지드바디

    void Awake()
    {
        PR = GetComponent<Rigidbody2D>();
        SetStatus(100, 100, 4);
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 curPos = transform.position;//현재위치
        

        if(h != 0 && !isDash && !isJump)
        {
            Dir = h;
            isMove = true;
        }
        else if(h == 0)
        {
            isMove = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            PR.velocity = Vector2.up * JumpForce;
            SetDirection(Dir);
            Invoke("JumpReset", 1f);
            Debug.Log("스페이스바 눌림");
        }
        else if (isJump)
        {
            if(h == Dir)
            {
                h = Dir;
            }
            else
            {
                h = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash) { 
            isDash = true;//대쉬했다를 체크
            Invoke("DashReset", 0.4f);
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 2 * MaxSpeed * Time.deltaTime;//대쉬할때 다음위치
            transform.position = curPos + DashPos;//더해서 위치변경
        }//대쉬


        //Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//키입력에 따른 다음 위치
        //transform.position = curPos + nextPos;//현재위치와 다음위치를 더함으로써 이동
        if (!isDash)
        {
            Vector2 newVel = new Vector2(h * MaxSpeed, PR.velocity.y);//플레이어 이동속도
            PR.velocity = newVel;//리지드바디에 속도 등록
        }

    }

    public void SetDirection(float h) 
    {
        Dir = h;
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
    }//대쉬이후

    private void JumpReset()
    {
        isJump = false;
    }//대쉬이후 초기화

    public void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
