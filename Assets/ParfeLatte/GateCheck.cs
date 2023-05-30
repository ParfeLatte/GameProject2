using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCheck : MonoBehaviour
{
    public GameObject Gate;//��
    public GameManager Manager;//���ӸŴ���

    private InteractObj interactobj;
    private BoxCollider2D col;

    public bool isOpen;//���ȴ���
    public bool isDestroy;//�ı��Ǿ�����
    public bool GateStat;// false�� ����, true�� ����
    public int GateLv;//������ ���ٷ���

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        interactobj = GetComponent<InteractObj>();
        animator.enabled = true;//�ִϸ����͸� �Ѽ� �� �ִϸ��̼� �������
        Gate.SetActive(true);//������������ �ݶ��̴� ������Ʈ�� ���� ��
        isOpen = false;//����
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
        OpenClose();//��������� Ȯ����
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isOpen = Manager.CheckGateOpen(GateLv);//�÷��̾� �±��� ������Ʈ�� ������ ���������� Ȯ��
        }
    }

    private void GateOpen()
    {
        animator.SetBool("isOpen", true);//������
        Gate.SetActive(false);//�ݶ��̴��� �ִ� ������Ʈ�� ���� ��Ȱ��ȭ �ؼ� ������ �� ����
        GateStat = true;
        Debug.Log(GateLv + "Lv ����Ʈ ���� ����, ���� �����ϴ�.");
    }

    private void GateClose()
    {
        Gate.SetActive(true);//������������ �ٽ� Ȱ��ȭ
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
}
