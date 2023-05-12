using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCheck : MonoBehaviour
{
    public GameObject Gate;//��
    public GameManager Manager;//���ӸŴ���

    public bool isOpen;//���ȴ���
    public bool GateStat;// false�� ����, true�� ����
    public int GateLv;//������ ���ٷ���

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;//�ִϸ����͸� �Ѽ� �� �ִϸ��̼� �������
        Gate.SetActive(true);//������������ �ݶ��̴� ������Ʈ�� ���� ��
        isOpen = false;//����
    }
    private void OpenClose()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.U))
        {
            switch (GateStat) {
                case false:
                    animator.SetBool("isOpen", true);//������
                    Gate.SetActive(false);//�ݶ��̴��� �ִ� ������Ʈ�� ���� ��Ȱ��ȭ �ؼ� ������ �� ����
                    GateStat = true;
                    Debug.Log(GateLv + "Lv ����Ʈ ���� ����, ���� �����ϴ�.");
                    break;
                case true:
                    Gate.SetActive(true);//������������ �ٽ� Ȱ��ȭ
                    animator.SetBool("isOpen", false);//��������� ���ݴ� �ִϸ��̼� ���
                    GateStat = false;
                    break;
            }
        }
    }

    void Update()
    {
        OpenClose();//��������� Ȯ����
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isOpen = Manager.CheckGateOpen(GateLv);//�÷��̾� �±��� ������Ʈ�� ������ ���������� Ȯ��
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isOpen = false;//���� ������ ����� ���� ����
        }
    }
}
