using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    //public float Speed;//�ִ� �̵��ӵ�
    //public float movetime;//�̵��� �ð� üũ
    //public float stoptime;
    public float Dir = 1;
    public float LastDir = 1; //������ ���̵������� ����
    public float JumpForce;//���� ����
    public float JumpTime;//�����ð�
    public float MaxJump;//�ִ���������
    public float h;//�̵�����

    public bool isDash;//�뽬�ߴ��� üũ
    public bool isMove;//���������� üũ
    public bool isJump;//���������� üũ

    Rigidbody2D PlayerRigid;

    void Awake()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();
        SetStatus(100, 100, 4);
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");//�̵������� �޾ƿ�

        if (h != 0 && !isDash && !isJump)
        {
            Dir = h;//���������� �������� ���� ����
            isMove = true;
        }//�����̴� ����
        else if (h == 0)
        {
            isMove = false;
        }
        Dash();//�뽬
        Jump();//����
        Move();//�̵�(�¿�)
    }

    public void Move()
    {
        if (!isDash)
        {
            Vector2 newVelocity = new(h * MoveSpeed, PlayerRigid.velocity.y);//�̵��ӵ�(���� x �ְ�ӵ�)
            PlayerRigid.velocity = newVelocity;//�̵��� �ε巴�� �ϱ� ���ؼ� �ӵ��� ����
        }
    }//�̵�

    private void Dash()
    {
        //�뽬
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDash == false)
        {
            isDash = true;//�뽬�ߴٸ� üũ
            Invoke("DashReset", 0.3f);//�뽬 �ð��� ���������� ���� 0.3��
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 3 * MoveSpeed * Time.deltaTime;
            transform.position += DashPos;
            isMove = true;
        }//�뽬Ű ������ isDash�� ������ �׵��� �뽬��(ó���� ������ �������� �뽬, �⺻�� �����ʹ����� 1)
        //�뽬�Ҷ��� �ٸ� �ൿ ����
    }//�뽬

    private void Jump()
    {
        //����
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            SaveLastDir(Dir);//������ �̵������� ���� ����(���������� ������ ����)
            PlayerRigid.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);//���� ���� ��
            Debug.Log("�����̽��� ����");
            Invoke("JumpReset", JumpTime);//��������
        }

        if (isJump)
        {
            isMove = true;

            if (h == 0 || h != LastDir)
            {
                h = 0;
            }
            else
            {
                h = LastDir;
            }
        }//�������϶� ���� ��ȯ ���� + ����� ������ ���ư��°� ����
    }//������ ���

    public bool GetMoveCheck()
    {
        return isMove;
    }

    public bool GetDashCheck()
    {
        return isDash;
    }//�뽬������ üũ�ؼ� ������

    private void SaveLastDir(float Dir) {
        LastDir = Dir;
    }//������ ������ ����

    private void DashReset()
    {
        isDash = false;
    }//�뽬���� �ʱ�ȭ

    private void JumpReset()
    {
        isJump = false;
    }//���� �ʱ�ȭ(�̰� ���߿� �� ���� �浹 �̺�Ʈ�� �ٲܰ���)

    void OnCollisionEnter()
    {

    }
}
