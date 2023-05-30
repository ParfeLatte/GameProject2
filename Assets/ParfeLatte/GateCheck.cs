using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCheck : MonoBehaviour
{
    public GameObject Gate;//문
    public GameManager Manager;//게임매니저

    private InteractObj interactobj;
    private BoxCollider2D col;

    public bool isOpen;//열렸는지
    public bool isDestroy;//파괴되었는지
    public bool GateStat;// false는 닫힘, true는 열림
    public int GateLv;//문의의 접근레벨

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        interactobj = GetComponent<InteractObj>();
        animator.enabled = true;//애니메이터를 켜서 문 애니메이션 재생가능
        Gate.SetActive(true);//못지나가도록 콜라이더 오브젝트인 문을 켬
        isOpen = false;//닫힘
        isDestroy = false;
    }
    private void OpenClose()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.F))
        {
            switch (GateStat) {
                case false:
                    GateOpen();
                    break;
                case true:
                    GateClose();
                    break;
            }
        }
    }

    void Update()
    {
        if (isDestroy) return;
        OpenClose();//열고닫음을 확인함
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isOpen = Manager.CheckGateOpen(GateLv);//플레이어 태그의 오브젝트가 들어오면 문열리는지 확인
        }
    }

    private void GateOpen()
    {
        animator.SetBool("isOpen", true);//문열림
        Gate.SetActive(false);//콜라이더가 있는 오브젝트인 문을 비활성화 해서 지나갈 수 있음
        GateStat = true;
        Debug.Log(GateLv + "Lv 게이트 접근 승인, 문이 열립니다.");
    }

    private void GateClose()
    {
        Gate.SetActive(true);//못지나가도록 다시 활성화
        animator.SetBool("isOpen", false);//역재생으로 문닫는 애니메이션 재생
        GateStat = false;
    }

    
    public void DestroyGate()
    {
        isDestroy = true;
        col.enabled = false;    
        interactobj.enabled = false;    
        GateOpen();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isOpen = false;//범위 내에서 벗어나면 문을 닫음
        }
    }
}
