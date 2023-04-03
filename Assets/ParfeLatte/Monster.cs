using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    //public GameObject small;//���� 15�� �ݶ��̴� ����
    //public GameObject Big;//���� 30�� �ݶ��̴� ����
    public GameObject Player;//�Ÿ�������
    public Transform AttackPos;//���ݹ��� ��ġ
    public Vector2 boxSize;//���ݹ���

    public int SleepState;//���� ������� 0:��������, 1:�߰�����, 2:��������, 3:���!!!

    public float moveTimeOne;//�÷��̾ �󸶳� �ɾ����� Ȯ��
    public float moveTimeTwo;//�÷��̾ �󸶳� �ɾ����� Ȯ��
    public float CheckTime;//���� �ȿ� ���� �ð�
    public float CoolTime;//���� ��Ÿ��
    public float AttackTime;//���ݽð�
    public float Dist;//���Ϳ� �÷��̾� ���� �Ÿ�
    public float Dir;//�̵�����
    public Vector3 dirVec;//���� ����

    public Player player;//�÷��̾� �ڵ�
    //public MonsterAttack Attack;//���� ���� ��ũ��Ʈ

    public bool isPlayerDash;//�÷��̾ �뽬�ߴ��� Ȯ��
    public bool isPlayerStop;//�÷��̾ ����°�?

    public Vector3 lastPlayerPosition;//��ġ �񱳿�


    private Rigidbody2D MR;
    private Animator animator;
    private SpriteRenderer MonsterRenderer;

    private Vector3 curPos;//������ġ
    private Vector2 AddPos = new Vector2(0, 3f);//pivot�� �Ʒ��� ���������Ƿ� ���� �˻綧 ����
    private Vector2 RayPos;
    private GameObject AttackPlayer;//

    private bool isMobMove = false;//���Ͱ� �����̴���

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MonsterRenderer = GetComponent<SpriteRenderer>();
        SleepState = 0;
        SetStatus(31, 10, 4.5f);//�ϴ� �Ϲݸ�����
        Health = MaxHealth;//ü���� �ִ�ü������ ���� 
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//�÷��̾�� ���� ���� �Ÿ�
        curPos = transform.position;//������ġ
        RayPos = MR.position + AddPos;
        if(SleepState != 3 & !isDead) { 
            StateCheck();
        }
        if(SleepState == 3 && !isDead) {
            OnWake();
            AttackCheck();
        }
        AttackTime += Time.deltaTime;
    }

    private void AttackCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(RayPos, dirVec, 2f, LayerMask.GetMask("Player"));
        if (rayHit.collider != null)
        {
            Player player = rayHit.collider.GetComponent<Player>();
            AttackPlayer = rayHit.collider.gameObject;
            Debug.Log("�÷��̾ �����Ÿ����� ����");
            if (AttackPlayer != null && AttackTime >= CoolTime)
            {
                isMobMove = false;
                animator.SetBool("isMove", false);
                animator.SetTrigger("Attack");
                Invoke("Attack", 0.15f);
                AttackTime = 0;
                Invoke("MoveAgain", 0.15f); ;
            }
            Debug.Log("�����մϴ�.");
        }
        else
        {
            AttackPlayer = null;
            Debug.Log("�����Ÿ����� �ƹ��� ����");
            Debug.Log("��� �����մϴ�");
        }
    }//����

    private void Attack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(AttackPos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                player.damaged(damage);
            }
        }
    }//������ ����
    public void MoveAgain()
    {
        isMobMove = true;
        animator.SetBool("isMove", true);
    }

    private void StateCheck()
    {
        if (Dist <= 30 && SleepState != 3)
        {
            if (SleepState == 0)
            {
                moveTimeOne += Time.deltaTime;
            }//���� �����϶� �˻��ϴ� �ð�
            else if (SleepState == 1 && Dist <= 15)
            {
                moveTimeTwo += Time.deltaTime;
            }//�߰� �����϶� �˻��ϴ� �ð�

            if (!player.isMove)
            {
                StartCoroutine("CheckStop");
            }//�������� ����˻�
            else
            {
                isPlayerStop = false;
            }//������ �ʾ���
            AreaCheck(SleepState);//����˻�!
        }
    }
    private void OnWake()
    {
        Debug.DrawRay(RayPos, dirVec * 2f, new Color(0, 1, 0));//����ĳ��Ʈ ǥ��(�Ÿ� Ȯ�ο�)
        if (isMobMove == true)
        {
            if (player.transform.position.x - gameObject.transform.position.x >= 0)
            {
                Dir = 1;
                dirVec = new Vector3(1f, 0f, 0f);
                MonsterRenderer.flipX = false;
            }
            else
            {
                Dir = -1;
                dirVec = new Vector3(-1, 0f, 0f);
                MonsterRenderer.flipX = true;
            }
            Vector3 NextPos = new Vector3(Dir, 0, 0) * MaxSpeed * Time.deltaTime;
            transform.position = curPos + NextPos;
        }
    }//�����ִ� ���� �ൿ�ϴ� ��ƾ

    IEnumerator CheckStop()
    {
        lastPlayerPosition = Player.transform.position;

        yield return new WaitForSeconds(0.1f);
        //Debug.Log("�˻��մϴ�.");
        if (lastPlayerPosition == Player.transform.position)
        {
            //Debug.Log("������ϴ�.");
            moveTimeOne = 0;
            moveTimeTwo = 0;
            isPlayerStop = true;
        }//�ح����Ƿ� �˻��ϴ� �ð��� �ʱ�ȭ
        else
        {
            //Debug.Log("��� �����̴� ���Դϴ�.");
        }//��� �������� Ȯ��
    }//��������� Ȯ���� 0.4�ʰ� �������� �ʾƾ� ����ɷ� �ӽ����� 
    
    public void AreaCheck(int state)
    {
        switch (state)
        {
            case 0:
                if (Dist <= 30)
                {
                    if (moveTimeOne >= 2.0)
                    {
                        SleepState = 1;
                        moveTimeOne = 0;
                        animator.SetInteger("SleepState", 1);
                        Debug.Log("�߰����� ���·� ���ϴ�.");
                    }
                }//�Ÿ��� 30�ȿ� ������ üũ(�����������)
                break;
            case 1:
                if (SleepState == 1 && Dist <= 15)
                {
                    if (moveTimeTwo >= 0.8)
                    {
                        SleepState = 2;
                        animator.SetInteger("SleepState", 2);
                        Debug.Log("�������� ���·� ���ϴ�.");
                    }
                    else if(isPlayerStop)
                    {
                        SleepState = 0;
                        animator.SetInteger("SleepState", 0);
                        Debug.Log("���� ���� ���·� ���ư��ϴ�.");
                    }
                }//�߰����� �����̰� 15�ȿ��� 0.8�ʰ� ��������?
                break;
            case 2:
                if (SleepState == 2)
                {
                    if (player.GetMoveCheck())
                    {
                        MonsterAwake();//���
                    }
                    else
                    {
                        SleepState = 1;
                        animator.SetInteger("SleepState", 1);
                        Debug.Log("�߰� ���� ���·� ���ư��ϴ�.");
                    }
                }//���� ���� ���� �� ���¿� �÷��̾ ������ �ٷ� ���
                break;
        }
        CheckPlayerDash();
    }
    public void CheckPlayerDash()
    {
        if (player.GetDashCheck() == true)
        {
            MonsterAwake();//���
        }
    }//�÷��̾ �뽬�ߴ��� üũ��

    private void MonsterAwake()
    {
        SleepState = 3;
        animator.SetInteger("SleepState", 3);
        animator.SetBool("isMove", true);
        isMobMove = true;
        Debug.Log("������ϴ�.");
    }

    public override void damaged(float damage)
    {
        Health -= damage;//ü�¿��� ��������ŭ ����
        Debug.Log("���� �ƾ���");
        //���
        //����

        if (Health <= 0 && !isDead)
        {
            Die();
        }//ü���� 0�� �ǰų� 0�Ʒ��� �������� ���� �ʾҴٸ�
        //�ǰݽ� ������ ����
    }

    public override void Die()
    {
        isMobMove = false;
        isDead = true;
        animator.SetTrigger("Die");
        Invoke("Destroy", 1.43f);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
/*�÷��̾�� �Ÿ��� 30���ϰ� �ȴ�  -> �˻���� -> 2�ʰ� ������ �߰�����
				          -> �뽬�ϸ� �ٷ� ���
�߰����鿡�� 15�Ÿ����� ���԰� �ȿ��� 0.8�ʰ� �ɾ��°�? ->  ���� ���� -> �������ڸ��� ���
->  �ƴϸ� �ٽ�  ���� ��������*/
//��->��->��->��(����)

//RaycastHit2D rayHit = Physics2D.Raycast(MR.position, dirVec, 1.2f, LayerMask.GetMask("Player"));
//if (rayHit.collider != null)
//{
//    LivingEntity player = rayHit.collider.GetComponent<Player>();
//    AttackPlayer = rayHit.collider.gameObject;
//    Debug.Log("�÷��̾ �����Ÿ����� ����");
//    if (AttackPlayer != null && !isAttack)
//    {
//        isAttack = true;
//        player.damaged(damage);
//        isAttack = false;
//    }
//    Debug.Log("�����մϴ�.");
//}
//else
//{
//    AttackPlayer = null;
//    Debug.Log("�����Ÿ����� �ƹ��� ����");
//    Debug.Log("��� �����մϴ�");
//}