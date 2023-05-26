using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public float AttackTime;//�����ѽð�
    public float CoolTime;//��Ÿ��
    public float movetime;//�̵��� �ð� üũ
    public float stoptime;//
    public float JumpForce;//������
    public float Dir;//�̵�����(�뽬�� �����ÿ� ���� ���ٲٰ���)
    public float h;//GeTAxisRaw�� �޴� ��
    public float stamina;//���׹̳�
    public float dashtime;//�뽬���� �󸶳� �������� üũ�ؼ� ���׹̳� ä��
    public float lastYpos;//������ ������ �������� �� ������ y��
    public float CurYpos;//���� �ÿ� ���� y��
    public float falltime;

    public bool isDash;//�뽬�ߴ��� üũ
    public bool isMove;//���������� üũ
    public bool isWall;//���� �ε������� üũ
    public bool isJump;//�����ߴ��� üũ
    public bool isCharge;//��������������
    public bool isFallDamage;//�����޾Ҵ���

    public Slider HealthSlider;//ü��UI
    public Slider StaminaSlider;//���׹̳�UI
    

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
        stamina = 100f;
        HealthSlider.value = Health;
        StaminaSlider.value = stamina;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * 1.75f, new Color(0, 1, 0));
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
            AttackCheck();//����
            Jump();//����
            Dash();//�뽬
            Move();//�̵�
            StaminaCheck();
            CheckFallDamage();
            dashtime += Time.deltaTime;
        }
    }

    private void AttackCheck()
    {
            if (Input.GetKeyDown(KeyCode.Z) && AttackTime >= CoolTime && !isCharge)
            {
                animator.SetBool("isCharge", true);
            }
            if (Input.GetKey(KeyCode.Z) && AttackTime >= CoolTime)//Ű�� �ӽ��� J�� ������ ���϶�
            {
                ChargeTime += Time.deltaTime;
                h = 0;

                isCharge = true;
                //Debug.Log("���� ��¡��");
            }
            if (Input.GetKeyUp(KeyCode.Z) && isCharge && AttackTime >= CoolTime)//Ű�� ������
            {
                isCharge = false;
                animator.SetBool("isCharge", false);
                if (ChargeTime < 1f)//��¡ �ð��� 1�� �����̸� �⺻����
                {
                    //Debug.Log("�⺻ ����");
                    Attack.GetAttack(damage);//PlayerAttack ��ũ��Ʈ�� �������� �������ְ� PlayerAttack������ ������ ������
                    animator.SetTrigger("Attack");//���� �ִϸ��̼� ���
                    AttackTime = 0;
                }
                else if (ChargeTime >= 1.0f)//��¡ �ð��� 1�� �̻��̸� ����
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJump)
        {
            isJump = true;//�����ߴ�
            PR.velocity = Vector2.up * JumpForce;//���� �ӷ��� ���� �����¸�ŭ ��
            SetDirection(Dir);//������ �̵������� ����(�ݴ�� ���� ���� ���ϰ�)
            animator.SetBool("isJump", true);
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
        if (!isDash || !isWall || !isCharge)
        {
            Vector2 newVel = new Vector2(h * MaxSpeed, PR.velocity.y);//�÷��̾� �̵��ӵ�
            PR.velocity = newVel;//������ٵ� �ӵ� ���
        }//�뽬���� �ƴҶ� �̵��ӵ��� �̰ɷ� ������
    }//�������� ó���ϴ� �Լ�

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash && stamina >= 25f)
        {
            isDash = true;//�뽬�ߴٸ� üũ
            animator.SetBool("isDash", true);
            stamina -= 25f;
            dashtime = 0f;
            Invoke("DashReset", 0.25f);
        }
        else if (isDash)
        {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 2 * MaxSpeed * Time.deltaTime;//�뽬�Ҷ� ������ġ
            transform.position = curPos + DashPos;//���ؼ� ��ġ����
        }
    }//�뽬 �Լ�
    
    private void StaminaCheck()
    {
        if(dashtime >= 1.25f && stamina <= 100f)
        {
            stamina += 5 * Time.deltaTime;
            if(stamina >= 100f)
            {
                stamina = 100f;
            }
        }
        StaminaSlider.value = stamina;
    }//���׹̳� ȸ���˻� 

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
        animator.SetBool("isDash", false);
    }//�뽬���� �ʱ�ȭ

    //private void JumpReset()
    //{
    //    isJump = false;
    //}//���� ���� �ʱ�ȭ

    public override void Die()
    {

        base.Die();
        animator.SetTrigger("Die");//�ִϸ����Ϳ� Die Ʈ���Ÿ� �����ؼ� ��� �ִϸ��̼� ���
        Invoke("Dead", 1.2f);//����Ŀ� ������Ʈ ��Ȱ��ȭ
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        HealthCheck();
    }

    public override void damaged(float damage)
    {
        base.damaged(damage);
        Debug.Log(damage);
        HealthCheck();
    }

    private void Dead()
    {
        gameObject.SetActive(false);//������Ʈ ��Ȱ��ȭ
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HealthCheck()
    {
        HealthSlider.value = Health;
    }

    //private void SetLastPos()
    //{
    //    lastYpos = transform.position.y;
    //    isFallDamage = false;
    //    Debug.Log("������");
    //}

    private void FallDamage(float time)
    {
       if(time >= 1.5f)
        {
            damaged(70f);
        }
        else
        {
            damaged(35f);
        }
    }
    private void CheckFallDamage()
    {
        RaycastHit2D FloorRay = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Floor"));
        if(FloorRay.collider == null)
        {
            falltime += Time.deltaTime;
        }
        else if(FloorRay.collider != null)
        {
            if(falltime > 1.0f)
            {
                FallDamage(falltime);
            }
            falltime = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
            animator.SetBool("isJump", false);
            //if (!isFallDamage)
            //{
            //    CheckFallDamage();  
            //}
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
        //if(col.gameObject.tag == "Floor")
        //{
        //    SetLastPos();
        //}
    }
}
//Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//Ű�Է¿� ���� ���� ��ġ
//transform.position = curPos + nextPos;//������ġ�� ������ġ�� �������ν� �̵�(���� ���� ������ ������)