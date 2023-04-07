using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessPoint : MonoBehaviour
{
    public KeyCard keycard;
    public GameManager gameManager;
    public KeyCardEvent Event;

    public int accessLevel;

    public bool isEvent = false;//이벤트가 실행중인지
    public bool CanAccess = false;//카드에 접근가능한지

    public event Action EventStart;

    void Awake()
    {
        EventStart += SetEvent;
        EventStart += Event.SirenOn;
        EventStart += Event.MobSpawn;
    }

    void Update()
    {
        
    }

    private void SetEvent()
    {
        isEvent = true;
        Debug.Log("이벤트가 시작됐습니다.");
    }

    public void EndEvent()
    {
        isEvent = false;
        CanAccess = true;
        Debug.Log("이벤트가 끝났습니다.\n키카드에 승인가능합니다.");
    }

    public void KeyCardCheck()
    {
        if (keycard.isHave)
        {
            gameManager.GetCard(keycard.AccessLevel, keycard.isHave);
            Debug.Log(keycard.KeyColor + "key card를 획득했습니다");
        }
        else
        {
            return;
        }
    }

    private void CheckAccess()
    {
        if(!isEvent && !CanAccess)//이벤트가 진행중이지 않고, 엑세스가 불가능하다면
        {
            EventStart();//이벤트 발생!!
            Debug.Log("이벤트 시작");
        }
        else if (isEvent)
        {
            Debug.Log("이벤트 중이므로 접근 거절");
            return;
        }
        else if(!isEvent && CanAccess)
        {
            keycard.AccessToKey();
            KeyCardCheck();
            Debug.Log("키카드에 접근 승인");
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
       if(col.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                CheckAccess();//엑세스 확인(첫 접근에는 이벤트 시작, 이벤트 중에는 접근 불가능, 이벤트 끝난 후에 키카드 획득 가능)
            }
        }
    }
}
