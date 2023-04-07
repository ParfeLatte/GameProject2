using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{ 
    public float movetime;//�̵��� �ð� üũ
    public float stoptime;
    public float JumpForce;//������
    public float Dir;//�̵�����(�뽬�� �����ÿ� ���� ���ٲٰ���)
    public float h;//GeTAxisRaw�� �޴� ��

    public bool isDash;//�뽬�ߴ��� üũ
    public bool isMove;//���������� üũ
    public bool isJump;//�����ߴ��� üũ

    //public PlayerAttack Attack;
    public PlayerAttack Attack;//������ �������ִ� ��ũ��Ʈ(���� ���� ������Ʈ�� �Ҵ�Ǿ� ������ ���⼭ ������ ������)

    private GameObject Enemy;//����


    private Rigidbody2D PR;//�÷��̾� ������ٵ�
    private SpriteRenderer PlayerRenderer;//��������Ʈ �¿� �ٲܶ� �������
    private Animator animator;//�ִϸ�����

    private float ChargeTime;//���� ��¡�ð�
    private Vector3 curPos;//���� ��ġ
    private Vector3 dirVec;//�ٶ󺸴� ����


    void Awake()
    {
        PR = GetComponent<Rigidbody2D>();//�Ҵ�
        animator = GetComponent<Animator>();//�Ҵ�
        PlayerRenderer = GetComponent<SpriteRenderer>();//�Ҵ�
        SetStatus(100, 15, 8);//������ ������
        Health = MaxHealth;//�����Ҷ� ���� ü���� �ִ� ü������ ��������
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        curPos = transform.position;//������ġ
        if(h != 0 && !isDash && !isJump)
        {
            Dir = h;
            dirVec = new Vector3(h, 0f, 0f);
            isMove = true;
            animator.SetBool("isMove", true);
        }
        else if(h == 0)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }//�����̴��� Ȯ����
        
        flipSpr();//�¿����
        Jump();//����
        Dash();//�뽬
        Move();//�̵�
        AttackCheck();//����
    }

    private void AttackCheck()
    {
        if (Input.GetKey(KeyCode.J))//Ű�� �ӽ��� J�� ������ ���϶�
        {
            ChargeTime += Time.deltaTime;
            //��¡ �ִϸ��̼�
            //Debug.Log("���� ��¡��");
        }
        if (Input.GetKeyUp(KeyCode.J))//Ű�� ������
        {
            if (ChargeTime <= 0.5f)//��¡ �ð��� 0.5�� �����̸� �⺻����
            {
                //Debug.Log("�⺻ ����");
                Attack.GetAttack(damage);//PlayerAttack ��ũ��Ʈ�� �������� �������ְ� PlayerAttack������ ������ ������
                //�⺻���� ���
            }
            else if (ChargeTime >= 1.0f)//��¡ �ð��� 1�� �̻��̸� ����
            {
                //Debug.Log("��ȭ ����");
                Attack.GetAttack(damage * 2.0f);//���� ������ 2���� �������� ����
                //���� ���
            }
            ChargeTime = 0;//��¡ Ÿ�� �ʱ�ȭ
        }
    }//���� üũ, ���ݹ�ư�� ������ ������ ��¡��!

    private void flipSpr()
    {
        if (!isDash && !isJump) {
            if (h == 1)
            {
                PlayerRenderer.flipX = false;//�������� �ٶ󺸵���
                
            }
            else if (h == -1)
            {
                PlayerRenderer.flipX = true;//������ �ٶ󺸵���
            }
        }
    }//���⿡ �°� ��������Ʈ ������
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;//�����ߴ�
            PR.velocity = Vector2.up * JumpForce;//���� �ӷ��� ���� �����¸�ŭ ��
            SetDirection(Dir);//������ �̵������� ����(�ݴ�� ���� ���� ���ϰ�)
            Invoke("JumpReset", 1f);//1�� �ڿ� ��������(�����Ҽ�������)
            //Debug.Log("�����̽��� ����");
        }
        else if (isJump)
        {
            if (h == Dir)
            {
                h = Dir;//������ �̵������� ����
            }
            else
            {
                h = 0;//������ ���� ��ȯ�� ���ϰԲ�
            }
        }
    }//������ ó���ϴ� �Լ�

    private void Move()
    {
        if (!isDash)
        {
            Vector2 newVel = new Vector2(h * MaxSpeed, PR.velocity.y);//�÷��̾� �̵��ӵ�
            PR.velocity = newVel;//������ٵ� �ӵ� ���
        }//�뽬���� �ƴҶ� �̵��ӵ��� �̰ɷ� ������
    }//�������� ó���ϴ� �Լ�

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash)
        {
            isDash = true;//�뽬�ߴٸ� üũ
            Invoke("DashReset", 0.4f);
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 2 * MaxSpeed * Time.deltaTime;//�뽬�Ҷ� ������ġ
            transform.position = curPos + DashPos;//���ؼ� ��ġ����
        }
    }//�뽬 �Լ�

    public void SetDirection(float h) 
    {
        Dir = h;//������ ���� ����(üũ�Ҷ� ���)
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
    }//�뽬���� �ʱ�ȭ

    private void JumpReset()
    {
        isJump = false;
    }//���� ���� �ʱ�ȭ
}
//Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//Ű�Է¿� ���� ���� ��ġ
//transform.position = curPos + nextPos;//������ġ�� ������ġ�� �������ν� �̵�(���� ���� ������ ������)