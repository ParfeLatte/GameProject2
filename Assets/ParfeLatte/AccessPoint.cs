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

    public bool isEvent;//이벤트가 실행중인지
    public bool isEnd;//이벤트를 끝냈는지
    public bool CanAccess;//카드에 접근가능한지

    void Awake()
    {
        isEvent = false;
        isEnd = false;
        CanAccess = false;
    }

    void Update()
    {
        
    }

    private void StartEvent()
    {
        Event.SirenOn();
        Event.MobSpawn();
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
            StartEvent();//이벤트 발생!!
            Debug.Log("이벤트 시작");
        }
        else if (isEvent && !CanAccess)
        {
            Debug.Log("이벤트 중이므로 접근 거부");
            return;
        }
        else if(!isEvent && CanAccess)
        {
            keycard.AccessToKey();
            KeyCardCheck();
            Debug.Log("키카드에 접근 승인");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("플레이어가 범위내에 있음 엑세스포인트에 접근 가능");
        if (col.tag == "Player" && Input.GetKeyDown(KeyCode.U))
        {
            CheckAccess();//엑세스 확인(첫 접근에는 이벤트 시작, 이벤트 중에는 접근 불가능, 이벤트 끝난 후에 키카드 획득 가능)
        }
    }
}
