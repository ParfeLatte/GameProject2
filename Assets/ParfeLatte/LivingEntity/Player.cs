using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Insomnia.Defines;
using static Insomnia.Player_Speaker;

public class Player : LivingEntity {
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
    public float m_playerAttackDamage = 10f;

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
    [SerializeField] private Player_Speaker m_speaker = null;

    private GameObject Enemy;//����


    private Rigidbody2D PR;//�÷��̾� ������ٵ�
    private SpriteRenderer PlayerRenderer;//��������Ʈ �¿� �ٲܶ� �������
    private Animator animator;//�ִϸ�����

    private float ChargeTime;//���� ��¡�ð�
    private Vector3 curPos;//���� ��ġ
    private Vector3 dirVec;//�ٶ󺸴� ����

    private Vector2 boxsize = new Vector2(1f, 1f);
    private bool m_canMove = true;
    public bool CanMove { get => m_canMove;
        set {
            m_canMove = value;
        }
    }

    void Awake() {
        PR = GetComponent<Rigidbody2D>();//�Ҵ�
        animator = GetComponent<Animator>();//�Ҵ�
        PlayerRenderer = GetComponent<SpriteRenderer>();//�Ҵ�
        //SoundEffect = GetComponent<PlayerSound>();
        m_speaker = GetComponentInChildren<Player_Speaker>();
        SetStatus(100, m_playerAttackDamage, 8);
        Health = MaxHealth;
        gameObject.SetActive(true);
        stamina = 100f;
        HealthSlider.value = Health;
        StaminaSlider.value = stamina;
    }

    private void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(GameManager.IsPause)
            return;

        if(CanMove == false)
            return;

        h = Input.GetAxisRaw("Horizontal");
        curPos = transform.position;
        if(h != 0 && !isDash && !isJump) {
            Dir = h;
            dirVec = new Vector3(h, 0f, 0f);
            isMove = true;
            animator.SetBool("isMove", true);
        }
        else if(h == 0) {
            isMove = false;
            animator.SetBool("isMove", false);
        }//�����̴��� Ȯ����

        AttackTime += Time.deltaTime;

        if(isFall) {
            Falltime += Time.deltaTime;
        }
        flipSpr();
        if(!isDead) {
            AttackCheck();
            Jump();
            Dash();
            Move();
            StaminaCheck();
            CheckFallDamage();
            CheckSoundEffect();
            dashtime += Time.deltaTime;
        }
    }

    private void CheckSoundEffect() {
        if(isJump || isDash || isCharge || isWall || isFall || h == 0) {
            m_speaker.Stop();
        }
        else if(isMove) {
            m_speaker.Play((int)PlayerSounds.PlayerWalk, true);
        }
    }
    private void AttackCheck() {
        if(Input.GetKeyDown(KeyCode.J) && AttackTime >= CoolTime && !isCharge) {
            animator.SetBool("isCharge", true);
        }
        if(Input.GetKey(KeyCode.J) && AttackTime >= CoolTime)
        {
            ChargeTime += Time.deltaTime;
            h = 0;

            isCharge = true;
        }
        if(Input.GetKeyUp(KeyCode.J) && isCharge && AttackTime >= CoolTime)
        {
            isCharge = false;
            animator.SetBool("isCharge", false);
            if(ChargeTime < 1f)
            {
                Attack.GetAttack(damage);
                animator.SetTrigger("Attack");
                AttackTime = 0;
            }
            else if(ChargeTime >= 1.0f)
            {
                Attack.GetAttack(damage * 3.0f);
                animator.SetTrigger("Attack");
                AttackTime = 0;
            }
            ChargeTime = 0;
            m_speaker.PlayOneShot((int)PlayerSounds.PlayerAttack_Normal);
        }
    }
    private void flipSpr() {
        if(!isDash && !isJump) {
            if(h == 1) {
                PlayerRenderer.flipX = false;

            }
            else if(h == -1) {
                PlayerRenderer.flipX = true;
            }
        }
    }
    private void Jump() {
        if(Input.GetKeyDown(KeyCode.W) && !isJump) {
            isJump = true;
            PR.velocity = Vector2.up * JumpForce;
            SetDirection(Dir);
            animator.SetBool("isJump", true);
            //Invoke("JumpReset", 0.8f);
        }
        else if(isJump) {
            if(h == Dir) {
                h = Dir;
            }
            else {
                h = 0;
            }
        }
    }

    private void Move() {
        if(!isDash || !isWall || !isCharge) {
            Vector2 newVel = new Vector2(h * MaxSpeed, PR.velocity.y);
            PR.velocity = newVel;

            if(newVel.magnitude >= 0.1f)
                m_speaker.Play((int)PlayerSounds.PlayerWalk, true);
            else
                m_speaker.Stop();
        }
    }

    private void Dash() {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isDash && stamina >= 25f) {
            isDash = true;
            animator.SetBool("isDash", true);
            stamina -= 25f;
            dashtime = 0f;
            Invoke("DashReset", 0.25f);
        }
        else if(isDash) {
            Vector3 DashPos = new Vector3(Dir, 0, 0) * 2 * MaxSpeed * Time.deltaTime;
            transform.position = curPos + DashPos;
        }
    }

    private void StaminaCheck() {
        if(dashtime >= 1.25f && stamina <= 100f) {
            stamina += 5 * Time.deltaTime;
            if(stamina >= 100f) {
                stamina = 100f;
            }
        }
        StaminaSlider.value = stamina;
    }

    public void SetDirection(float h) {
        Dir = h;
    }

    public bool GetMoveCheck() {
        return isMove;
    }

    public bool GetDashCheck() {
        return isDash;
    }

    private void DashReset() {
        isDash = false;
        animator.SetBool("isDash", false);
    }
    public override void Die() {

        base.Die();
        animator.SetTrigger("Die");
        Invoke("Dead", 0.5f);
    }

    public override void RestoreHealth(float newHealth) {
        base.RestoreHealth(newHealth);
        HealthCheck();
    }

    public override void damaged(float damage)
    {
        if (!isDash)
        {
            base.damaged(damage);
        }
        HealthCheck();
    }

    private void Dead() {
        //gameObject.SetActive(false);
        BGM_Speaker.Instance.Stop();
        ItemManager.Instance.RemoveAllItemDatas(this);
        PlayerPrefs.SetString("Failed", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        SceneController.Instance.ChangeSceneTo("EscapeFailed", true);
    }

    private void HealthCheck() {
        HealthSlider.value = Health;
    }

    //private void SetLastPos()
    //{
    //    lastYpos = transform.position.y;
    //    isFallDamage = false;
    //}

    private void FallDamage(float time) {
        if(time < 1.12f) return;

        if(time >= 1.5f) {
            damaged(100f);
        }
        else if(time >= 1.12f) {
            damaged(50f);
        }
    }
    private void CheckFallDamage() {
        Collider2D col = Physics2D.OverlapBox(transform.position, boxsize, 0, LayerMask.GetMask("Floor"));
        if(col != null) {
            FallDamage(Falltime);
            Falltime = 0f;
            isFall = false;
        }
        else if(col == null) {
            isFall = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxsize);
    }
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Floor") {
            isJump = false;
            animator.SetBool("isJump", false);
        }
        if(col.gameObject.tag == "Wall") {
            h = 0;
            Dir = 0;
            isWall = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col) {
        if(col.gameObject.tag == "Wall") {
            Dir = 0;
            h = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if(col.gameObject.tag == "Wall") {
            isWall = false;
        }
    }
}
//Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;
//transform.position = curPos + nextPos;