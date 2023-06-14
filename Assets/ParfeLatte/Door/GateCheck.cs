using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Door_Speaker;

public class GateCheck : SearchableBase
{
    public GameObject Gate;

    private InteractObj interactobj;
    private BoxCollider2D col;

    public bool isOpen;
    public bool isDestroy;
    public bool GateStat;
    public int GateLv;

    [SerializeField] private Door_Speaker m_speaker = null;
    private Animator animator;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        interactobj = GetComponent<InteractObj>();
        m_speaker = GetComponentInChildren<Door_Speaker>();
        animator.enabled = true;
        Gate.SetActive(true);
        isOpen = false;
        isDestroy = false;
    }


    protected override void Start() {
        base.Start();
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
        if(GameManager.IsPause)
            return;

        if(isDestroy)
            return;

        OpenClose();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //if (col.tag == "Player")
        //{
        //    isOpen = Manager.CheckGateOpen(GateLv);//�÷��̾� �±��� ������Ʈ�� ������ ���������� Ȯ��
        //}
    }

    private void GateOpen()
    { 
        animator.SetBool("isOpen", true);//������
        //Sound.OpenSound();
        m_speaker.PlayOneShot((int)DoorSounds.DoorOpen);
        Gate.SetActive(false);//�ݶ��̴��� �ִ� ������Ʈ�� ���� ��Ȱ��ȭ �ؼ� ������ �� ����
        GateStat = true;
    }

    private void GateClose()
    {
        Gate.SetActive(true);//������������ �ٽ� Ȱ��ȭ
        //Sound.CloseSound();
        m_speaker.PlayOneShot((int)DoorSounds.DoorClose);
        animator.SetBool("isOpen", false);//��������� ���ݴ� �ִϸ��̼� ���
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
            isOpen = false;//���� ������ ����� ���� ����
        }
    }

    protected override void Reset() {
        
    }
}
