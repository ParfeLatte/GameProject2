using Insomnia;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

[Obsolete("No Use Anymore", true)]
public class AccessPoint : SearchableBase {
    public KeyCard keycard;
    public KeyCardEvent Event;

    public int accessLevel; //키카드 레벨

    public bool isEvent;    //이벤트가 실행중인지
    public bool isEnd;      //이벤트를 끝냈는지
    public bool CanAccess;  //카드에 접근가능한지

    private bool isPlayer;  //플레이어가 접근했는지

    protected override void Awake()
    {
        base.Awake();
        isEvent = false;
        isEnd = false;
        CanAccess = false;
        isPlayer = false;
        accessLevel = keycard.AccessLevel;
    }

    protected override void Start() {
        base.Start();
    }

    void Update()
    {
        if(GameManager.IsPause)
            return;

        if(isPlayer && Input.GetKeyDown(KeyCode.F)){
            switch (accessLevel)
            {
                case 4:
                    TargetCheck();
                    break;
                default:
                    CheckAccess();//플레이어가 범위 내에 있을때 F를 눌렀으면 접근시도
                    break;
            }
        }
    }

    public void TargetCheck()
    {
        //if (gameManager.Target())
        //{
        //    CheckAccess();
        //}
        //else
        //{
        //    Debug.Log("타겟이 없으므로 접근불가");
        //}
    }

    public void KeyCardCheck()
    {
        //if (keycard.isHave)
        //{
        //    gameManager.GetCard(keycard.AccessLevel, keycard.isHave);//키카드를 가지고있으면 게임매니저에게 전달
        //    Debug.Log(keycard.KeyColor + "key card를 획득했습니다");
        //}
        //else
        //{
        //    return;//없음
        //}
    }

    public void EndEvent()
    {
        isEvent = false;//이벤트가 끝남
        CanAccess = true;//키카드에 접근가능
    }

    private void StartEvent()
    {
        Event.SirenOn();//사이렌 재생
        isEvent = true;
    }

    private void CheckAccess()
    {
        if(!isEvent && !CanAccess)//이벤트가 진행중이지 않고, 엑세스가 불가능하다면
        {
            StartEvent();//이벤트 발생!!
        }
        else if (isEvent && !CanAccess)
        {
            return;//접근 못하도록 리턴
        }
        else if(!isEvent && CanAccess)
        {
            keycard.AccessToKey();//키카드 획득
            KeyCardCheck();//키카드를 획득했음을 게임매니저에 전달
            gameObject.SetActive(false);//키카드 제거(오브젝트에서)
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isPlayer = true;//엑세스 확인(첫 접근에는 이벤트 시작, 이벤트 중에는 접근 불가능, 이벤트 끝난 후에 키카드 획득 가능)
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isPlayer = false;//엑세스 확인(첫 접근에는 이벤트 시작, 이벤트 중에는 접근 불가능, 이벤트 끝난 후에 키카드 획득 가능)
        }
    }

    protected override void Reset() {
        
    }
}
