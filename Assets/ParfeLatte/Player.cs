using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : LivingEntity
{
    public float AttackTime;//�����ѽð�
    public float CoolTime;//��Ÿ��
    public float movetime;//�̵��� �ð� üũ
    public float stoptime;
    public float JumpForce;//������
    public float Dir;//�̵�����(�뽬�� �����ÿ� ���� ���ٲٰ���)
    public float h;//GeTAxisRaw�� �޴� ��

    public bool isDash;//�뽬�ߴ��� üũ
    public bool isMove;//���������� üũ
    public bool isWall;//���� �ε������� üũ
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
        SetStatus(100, 10, 8);//������ ������ ü��, ������, �̵��ӵ�
        Health = MaxHealth;//�����Ҷ� ���� ü���� �ִ� ü������ ��������
        gameObject.SetActive(true);
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

        AttackTime += Time.deltaTime;
        
        flipSpr();//�¿����
        if (!isDead)
        {
            Jump();//����
            Dash();//�뽬
            Move();//�̵�
            AttackCheck();//����
        }
    }

    private void AttackCheck()
    {
        if (Input.GetKey(KeyCode.J))//Ű�� �ӽ��� J�� ������ ���϶�
        {
            ChargeTime += Time.deltaTime;
            //��¡ �ִϸ��̼�
            //Debug.Log("���� ��¡��");
        }
        if (Input.GetKeyUp(KeyCode.J) && AttackTime >= CoolTime)//Ű�� ������
        {
            if (ChargeTime < 1f)//��¡ �ð��� 1�� �����̸� �⺻����
            {
                //Debug.Log("�⺻ ����");
                Attack.GetAttack(damage);//PlayerAttack ��ũ��Ʈ�� �������� �������ְ� PlayerAttack������ ������ ������
                animator.SetTrigger("Attack");//���� �ִϸ��̼� ���
                AttackTime = 0;
            }
            else if (ChargeTime >= 1.0f && AttackTime >= CoolTime)//��¡ �ð��� 1�� �̻��̸� ����
            {
                //Debug.Log("��ȭ ����");
                Attack.GetAttack(damage * 3.0f);//���� ������ 3���� �������� ����
                animator.SetTrigger("Attack");//���� �ִϸ��̼� ���
                AttackTime = 0;
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
            //Invoke("JumpReset", 0.8f);//1�� �ڿ� ��������(�����Ҽ�������)
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
        if (!isDash || !isWall)
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
            Invoke("DashReset", 0.25f);
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

    //private void JumpReset()
    //{
    //    isJump = false;
    //}//���� ���� �ʱ�ȭ

    public override void Die()
    {
        
        isDead = true;//�������
        animator.SetTrigger("Die");//�ִϸ����Ϳ� Die Ʈ���Ÿ� �����ؼ� ��� �ִϸ��̼� ���
        Invoke("Dead", 1.2f);//����Ŀ� ������Ʈ ��Ȱ��ȭ
    }

    private void Dead()
    {
        gameObject.SetActive(false);//������Ʈ ��Ȱ��ȭ
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
        }
        if (col.gameObject.tag == "Wall")
        {
            h = 0;
            Dir = 0;
            isWall = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Dir = 0;
            h = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            isWall = false;
        }
    }
}
//Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//Ű�Է¿� ���� ���� ��ġ
//transform.position = curPos + nextPos;//������ġ�� ������ġ�� �������ν� �̵�(���� ���� ������ ������)