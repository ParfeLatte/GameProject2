using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKeyGet : MonoBehaviour
{
    public KeyCard keycard;
    public int accessLevel; //키카드 레벨
    public bool CanAccess;  //카드에 접근가능한지
    private bool isPlayer;  //플레이어가 접근했는지
    // Start is called before the first frame update
    void Awake()
    {
        CanAccess = false;
        isPlayer = false;
        accessLevel = keycard.AccessLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if (isPlayer && Input.GetKeyDown(KeyCode.F))
        {
            GetTutorialCard();
        }
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

    private void GetTutorialCard()
    {
        keycard.AccessToKey();//키카드 획득
        KeyCardCheck();//키카드를 획득했음을 게임매니저에 전달
        gameObject.SetActive(false);//키카드 제거(오브젝트에서)
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
}
