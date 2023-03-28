using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{ 
    public float movetime;//�̵��� �ð� üũ
    public float stoptime;
    public float JumpForce;
    public float Dir;//�̵�����(�뽬�� �����ÿ� ���� ���ٲٰ���)

    public bool isDash;//�뽬�ߴ��� üũ
    public bool isMove;//���������� üũ
    public bool isJump;//�����ߴ��� üũ

    private Rigidbody2D PR;//�÷��̾� ������ٵ�

    void Awake()
    {
        PR = GetComponent<Rigidbody2D>();
        SetStatus(100, 100, 4);
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 curPos = transform.position;//������ġ
        

        if(h != 0 && !isDash && !isJump)
        {
            Dir = h;
            isMove = true;
        }
        else if(h == 0)
        {
            isMove = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            PR.velocity = Vector2.up * JumpForce;
            SetDirection(Dir);
            Invoke("JumpReset", 1f);
            Debug.Log("�����̽��� ����");
        }
        else if (isJump)
        {
            if(h == Dir)
            {
                h = Dir;
            }
            else
            {
                h = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash) { 
            isDash = true;//�뽬�ߴٸ� üũ
            Invoke("DashReset", 0.4f);
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 2 * MaxSpeed * Time.deltaTime;//�뽬�Ҷ� ������ġ
            transform.position = curPos + DashPos;//���ؼ� ��ġ����
        }//�뽬


        //Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//Ű�Է¿� ���� ���� ��ġ
        //transform.position = curPos + nextPos;//������ġ�� ������ġ�� �������ν� �̵�
        if (!isDash)
        {
            Vector2 newVel = new Vector2(h * MaxSpeed, PR.velocity.y);//�÷��̾� �̵��ӵ�
            PR.velocity = newVel;//������ٵ� �ӵ� ���
        }

    }

    public void SetDirection(float h) 
    {
        Dir = h;
    }

    public bool GetMoveCheck()
    {
        return isMove;
    }

    public bool GetDashCheck()
    {
        return isDash;
    }//�뽬������ üũ�ؼ� ������

    private void DashReset()
    {
        isDash = false;
    }//�뽬����

    private void JumpReset()
    {
        isJump = false;
    }//�뽬���� �ʱ�ȭ

    public void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
