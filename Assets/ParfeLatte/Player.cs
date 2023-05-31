using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Insomnia.Defines;

public class Player : LivingEntity, IDataIO
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
    public float Falltime;

    public bool isDash;//�뽬�ߴ��� üũ
    public bool isMove;//���������� üũ
    public bool isWall;//���� �ε������� üũ
    public bool isJump;//�����ߴ��� üũ
    public bool isCharge;//��������������
    public bool isFall;//�����޾Ҵ���

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

    private Vector2 boxsize = new Vector2(1f, 1f);

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

    private void Start() {
        GameManager.Instance.AddPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

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
        
        if (isFall)
        {
            Falltime += Time.deltaTime;
        }
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
            if (Input.GetKeyDown(KeyCode.J) && AttackTime >= CoolTime && !isCharge)
            {
                animator.SetBool("isCharge", true);
            }
            if (Input.GetKey(KeyCode.J) && AttackTime >= CoolTime)//Ű�� �ӽ��� J�� ������ ���϶�
            {
                ChargeTime += Time.deltaTime;
                h = 0;

                isCharge = true;
                //Debug.Log("���� ��¡��");
            }
            if (Input.GetKeyUp(KeyCode.J) && isCharge && AttackTime >= CoolTime)//Ű�� ������
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
        if (Input.GetKeyDown(KeyCode.W) && !isJump)
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
        Invoke("Dead", 0.8f);//����Ŀ� ������Ʈ ��Ȱ��ȭ
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
        //gameObject.SetActive(false);//������Ʈ ��Ȱ��ȭ
        ItemManager.Instance.RemoveAllItemDatas(this);
        SceneController.Instance.ChangeSceneTo("EscapeFailed", true);
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
        if(time < 1f) return;

        if(time >= 1.5f)
        {
            damaged(100f);
        }
        else if(time >= 1f)
        {
            damaged(50f);
        }
    }
    private void CheckFallDamage()
    {
        Collider2D col = Physics2D.OverlapBox(transform.position, boxsize, 0, LayerMask.GetMask("Floor"));
        if(col != null)
        {
            Debug.Log("���鿡 �ֽ��ϴ�");
            FallDamage(Falltime);
            Falltime = 0f;
            isFall = false;
        }
        else if(col == null)
        {
            Debug.Log("���߿� ��");
            isFall = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxsize);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
            animator.SetBool("isJump", false);
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

    private void OnApplicationQuit() {
        RemoveData();
    }

    public void SaveData() {
        string jsonData = JsonUtility.ToJson(transform.position);
        PlayerPrefs.SetString("PlayerPosition", jsonData);
        PlayerPrefs.SetFloat("PlayerHealth", Health);
        PlayerPrefs.SetFloat("PlayerStamina", stamina);
    }

    public void LoadData() {
        if((PlayerPrefs.HasKey("PlayerPosition") && PlayerPrefs.HasKey("PlayerHealth") && PlayerPrefs.HasKey("PlayerStamina") ) == false)
            return;

        string positionData = PlayerPrefs.GetString("PlayerPosition");
        if(positionData == string.Empty || positionData == "")
            return;

        Vector3 position = JsonUtility.FromJson<Vector3>(positionData);
        if(position == null)
            return;

        transform.position = position;

        Health = PlayerPrefs.GetFloat("PlayerHealth");
        stamina = PlayerPrefs.GetFloat("PlayerStamina");
    }

    public void RemoveData() {
        PlayerPrefs.DeleteKey("PlayerPosition");
        PlayerPrefs.DeleteKey("PlayerHealth");
        PlayerPrefs.DeleteKey("PlayerStamina");
        PlayerPrefs.Save();
    }
}
//Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//Ű�Է¿� ���� ���� ��ġ
//transform.position = curPos + nextPos;//������ġ�� ������ġ�� �������ν� �̵�(���� ���� ������ ������)