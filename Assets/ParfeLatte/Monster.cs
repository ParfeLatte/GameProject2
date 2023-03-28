using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    //public GameObject small;//범위 15의 콜라이더 영역
    //public GameObject Big;//범위 30의 콜라이더 영역
    public GameObject Player;//거리측정용

    public int SleepState;//현재 수면상태 0:깊은수면, 1:중간수면, 2:얕은수면, 3:기상!!!

    public float moveTimeOne;//플레이어가 얼마나 걸었는지 확인
    public float moveTimeTwo;//플레이어가 얼마나 걸었는지 확인
    public float CheckTime;//범위 안에 들어온 시간
    public float Dist;//몬스터와 플레이어 사이 거리
    public float Dir;//이동방향
    
    
    public Vector3 lastPlayerPosition;//

    private Rigidbody2D MR;

    public Player player;//플레이어 코드

    public bool isPlayerDash;//플레이어가 대쉬했는지 확인
    public bool isPlayerStop;//플레이어가 멈췄는가?

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        SleepState = 0;
        SetStatus(100, 100, 4);
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//플레이어와 몬스터 사이 거리
        Vector3 curPos = transform.position;//현재위치
        

        if (Dist <= 30 && SleepState != 3)
        {
            if(SleepState == 0)
            {
                moveTimeOne += Time.deltaTime;
            }//깊은 수면일때 검사하는 시간
            else if(SleepState  == 1 && Dist <= 15)
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

        if(SleepState == 3)
        {
            if(player.transform.position.x - gameObject.transform.position.x >= 0)
            {
                Dir = 1;
            }
            else
            {
                Dir = -1;
            }
            Vector3 NextPos = new Vector3(Dir, 0, 0) * MaxSpeed * Time.deltaTime;
            transform.position = curPos + NextPos;
        }
       
    }

    IEnumerator CheckStop()
    {
        lastPlayerPosition = Player.transform.position;

        yield return new WaitForSeconds(0.1f);
        Debug.Log("검사합니다.");
        if (lastPlayerPosition == Player.transform.position)
        {
            Debug.Log("멈췄습니다.");
            moveTimeOne = 0;
            moveTimeTwo = 0;
            isPlayerStop = true;
        }//멈췃으므로 검사하던 시간들 초기화
        else
        {
            Debug.Log("계속 움직이는 중입니다.");
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
                    }
                }//거리가 30안에 들어오면 체크(깊은수면상태)
                break;
            case 1:
                if (SleepState == 1 && Dist <= 15)
                {
                    if (moveTimeTwo >= 0.8)
                    {
                        SleepState = 2;
                    }
                    else if(isPlayerStop)
                    {
                        SleepState = 0;
                    }
                }//중간수면 상태이고 15안에서 0.8초간 움직였나?
                break;
            case 2:
                if (SleepState == 2)
                {
                    if (player.GetMoveCheck())
                    {
                        SleepState = 3;
                    }
                    else
                    {
                        SleepState = 1;
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
            SleepState = 3;
        }
    }//플레이어가 대쉬했는지 체크함
}
/*플레이어와 거리가 30이하가 된다  -> 검사시작 -> 2초간 걸으면 중간수면
				          -> 대쉬하면 바로 기상
중간수면에서 15거리까지 들어왔고 안에서 0.8초간 걸었는가? ->  얕은 수면 -> 움직이자마자 기상
->  아니면 다시  깊은 수면으로*/
//초->노->빨->빨(유지)