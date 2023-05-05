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
    //public float moveTimeTwo;//�÷��̾ �󸶳� �ɾ����� Ȯ��
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
    public bool isAttack;//�������ΰ�?
    public bool isWall;//���� �ε����°�?

    public Vector3 lastPlayerPosition;//��ġ �񱳿�


    private Rigidbody2D MR;
    private Animator animator;
    private SpriteRenderer MonsterRenderer;

    private Vector3 curPos;//������ġ
    private Vector2 AddPos = new Vector2(0, 3f);//pivot�� �Ʒ��� ���������Ƿ� ���� �˻綧 ����
    private Vector2 RayPos;//���� ����
    private GameObject AttackPlayer;//���ݴ��

    private bool isMobMove = false;//���Ͱ� �����̴���

    void Awake()
    {
        MR = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MonsterRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.Find("Player");
        SleepState = 0;//�������� ���·� ����
        SetStatus(30, 10, 4.5f);//�ϴ� �Ϲݸ�����
        Health = MaxHealth;//ü���� �ִ�ü������ ���� 
    }

    private void OnEnable()
    {
        isDead = false;
        SetStatus(30, 10, 4.5f);//�ϴ� �Ϲݸ�����
        Health = MaxHealth;//ü���� �ִ�ü������ ���� 
    }

    // Update is called once per frame
    void Update()
    {
        Dist = Mathf.Abs(Player.transform.position.x - gameObject.transform.position.x);//�÷��̾�� ���� ���� �Ÿ�
        if (Mathf.Abs(Player.transform.position.y - gameObject.transform.position.y) < 5)
        {
            if (SleepState != 3 & !isDead)
            {
                StateCheck();//���� ��������̹Ƿ� ������� Ȯ��
            }//����� ����
        }
        curPos = transform.position;//������ġ
        RayPos = MR.position + AddPos;//���̸� �߻��ϴ� ��ġ
        if (SleepState == 3 && !isDead) {
            OnWake();//���������� �ൿ����
            AttackCheck();//���ݹ��� ���� ������ ����
            AttackTime += Time.deltaTime;//��Ÿ��
        }//���� ����
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(AttackPos.position, boxSize);
    //}

    private void AttackCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(RayPos, dirVec, 1.75f, LayerMask.GetMask("Player"));//Ray�� �߻��Ͽ� �÷��̾ �������� �ִ��� Ȯ��
        if (rayHit.collider != null)//���̿� �浹�� ����� ������
        {
            Player player = rayHit.collider.GetComponent<Player>();//�÷��̾� ������Ʈ �Ҵ�
            AttackPlayer = rayHit.collider.gameObject;//������ �÷��̾� ��� �Ҵ�
            //Debug.Log("�÷��̾ �����Ÿ����� ����");
            if (AttackPlayer != null && AttackTime >= CoolTime)//���� ��Ÿ���� ���Ұ�, �÷��̾ �Ҵ�Ǿ��ִٸ�
            {
                isMobMove = false;//��� �������� ���߰�
                animator.SetBool("isMove", false);//�ִϸ��̼� �Ķ���Ϳ��� isMove�� false�� �ٲٰ�
                animator.SetTrigger("Attack");//attack Trigger�� �ߵ��ؼ� ���� �ִϸ��̼� ���
                Invoke("Attack", 0.38f);//�ִϸ��̼ǿ��� �ֵθ��� ��ǿ� �°� ����
                AttackTime = 0;//�ٽ� ��Ÿ��
                Invoke("MoveAgain", 0.55f);//�����Ŀ� �ٽ� �����̵���
            }
            //Debug.Log("�����մϴ�.");
        }
        else
        {
            AttackPlayer = null;
            //Debug.Log("�����Ÿ����� �ƹ��� ����");
            //Debug.Log("��� �����մϴ�");
        }//����ó��
    }//����

    private void Attack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(AttackPos.position, boxSize, 0);//������ �ڽ��� �̿�
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")//�������ڽ��� �˻��� ������Ʈ �߿��� �÷��̾ ������
            {
                player.damaged(damage);//�÷��̾��� damaged �Լ� ȣ���ؼ� �������� ��
            }
        }
    }//������ ����

    public void MoveAgain()
    { 
        animator.SetBool("isMove", true);//�̵� �ִϸ��̼� ��
        isMobMove = true;//�ٽ� ������
    }

    private void StateCheck()
    {
        if (SleepState != 3)
        {
            if (SleepState == 1)
            {
                moveTimeOne += Time.deltaTime;//20�� �������� ������ �˻��ϴ� �ð�
            }//���� �����϶� �˻��ϴ� �ð�
            if (!player.isMove)
            {
                StartCoroutine("CheckStop");//�÷��̾��� �������� ������� �˻�
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
        Debug.DrawRay(RayPos, dirVec * 1.75f, new Color(0, 1, 0));//����ĳ��Ʈ ǥ��(�Ÿ� Ȯ�ο�)
        if (isMobMove == true)
        {
            
            if (player.transform.position.x - gameObject.transform.position.x > 0)
            {
                Dir = 1;//���� ������
                dirVec = new Vector3(1f, 0f, 0f);//Ray �߻����
                MonsterRenderer.flipX = false;//��������Ʈ ����x(������ ��)
            }
            else if((player.transform.position.x - gameObject.transform.position.x < 0))
            {
                Dir = -1;//���� ����
                dirVec = new Vector3(-1, 0f, 0f);//Ray �߻����
                MonsterRenderer.flipX = true;//��������Ʈ ���� O(���ʺ���)
            }

            if (Dist <= 1.3f)
            {
                Dir = 0;
            }
            Vector3 NextPos = new Vector3(Dir, 0, 0) * MaxSpeed * Time.deltaTime;
            transform.position = curPos + NextPos;//�÷��̾� ���ؼ� �̵�(����)
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
            moveTimeOne = 0;//�÷��̾ ������ �ð� �ʱ�ȭ
            //moveTimeTwo = 0;//�÷��̾ ������ �ð� �ʱ�ȭ
            isPlayerStop = true;//�÷��̾ ������ϴ�.
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
            case 0://�������� �����϶� �˻�
                if(Dist <= 20)
                {
                    SleepState = 1;//�߰���������
                    animator.SetInteger("SleepState", 1);
                }
                break;
            case 1://�������� �����϶� �˻�
                if (Dist <= 20)
                {
                    if (moveTimeOne >= 0.8f)//0.8�� �̻� �������ٸ�
                    {
                        MonsterAwake();
                    }//�Ÿ��� 30�ȿ� ������ üũ(�����������)

                    CheckPlayerDash();//�÷��̾ �뽬������ Ȯ����.
                }
                else if (Dist >= 20)
                {
                    SleepState = 0;//���� �������� ���ư�
                    moveTimeOne = 0;
                    animator.SetInteger("SleepState", 0);
                }
                break;
        }
    }

    public void CheckPlayerDash()
    {
        if (player.GetDashCheck() == true)//�÷��̾ �뽬�� �ߴٸ�
        {
            MonsterAwake();//���
        }
    }//�÷��̾ �뽬�ߴ��� üũ��

    public void MonsterAwake()
    {
        SleepState = 3;//��� ����
        animator.SetInteger("SleepState", 3);//�ִϸ��̼��� ��� ���·� ����
        //MonsterRenderer.color = new Color(1, 0f, 0f, 1f);
        animator.SetBool("isMove", true);//�÷��̾ �����ؿ��� ������ isMove �Ķ���Ͱ��� true�� ����
        isMobMove = true;//���ʹ� �����δ�.
        //Debug.Log("������ϴ�.");
    }

    public override void damaged(float damage)
    {
        Health -= damage;//ü�¿��� ��������ŭ ����
        //Debug.Log("���Ͱ� �������� �Ծ����ϴ�.");
        //���
        //����

        if(SleepState != 3)
        {
            MonsterAwake();//�������϶� ���ݴ��ϸ� ���
        }

        if (Health <= 0 && !isDead)
        {
            Die();//���
        }//ü���� 0�� �ǰų� 0�Ʒ��� �������� ���� �ʾҴٸ�
        //�ǰݽ� ������ ����
    }

    public override void Die()
    {
        isMobMove = false;//������ ����
        isDead = true;//�������
        animator.SetTrigger("Die");//�ִϸ����Ϳ� Die Ʈ���Ÿ� �����ؼ� ��� �ִϸ��̼� ���
        Invoke("Destroy", 1.2f);//����Ŀ� ������Ʈ ��Ȱ��ȭ
    }

    private void Destroy()
    {
        gameObject.SetActive(false);//������Ʈ ��Ȱ��ȭ
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Dir = 0;
            isWall = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Wall")
        {
            Dir = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            isWall = false;
        }
    }
}
/*�÷��̾�� �Ÿ��� 30���ϰ� �ȴ�  -> �˻���� -> 2�ʰ� ������ �߰�����
				          -> �뽬�ϸ� �ٷ� ���
�߰����鿡�� 15�Ÿ����� ���԰� �ȿ��� 0.8�ʰ� �ɾ��°�? ->  ���� ���� -> �������ڸ��� ���
->  �ƴϸ� �ٽ�  ���� ��������*/
//��->��->��->��(����)


//case 2://�߰����� �����϶� �˻�
//    if (SleepState == 1 && Dist <= 15)//�߰����� �����̰�, �÷��̾ 15�� �������� ������
//    {
//        if (moveTimeTwo >= 0.8)//0.8�� �̻� ������
//        {
//            SleepState = 2;//���� ���� ���·� ��
//            moveTimeTwo = 0;//2�� �˻�ð� �ʱ�ȭ
//            animator.SetInteger("SleepState", 2);//���� ���� �ִϸ��̼����� ����
//            //Debug.Log("�������� ���·� ���ϴ�.");
//        }
//        else if(isPlayerStop)//���� ����ٸ�
//        {
//            SleepState = 0;//�ٽ� ���� ���� ���·� ��
//            //MonsterRenderer.color = new Color(1, 1, 1, 1);
//            animator.SetInteger("SleepState", 0);//���� ���� ���·� �ִϸ��̼� ��ȯ
//            //Debug.Log("���� ���� ���·� ���ư��ϴ�.");
//        }
//    }//�߰����� �����̰� 15�ȿ��� 0.8�ʰ� ��������?
//    break;
//case 2://���� ���� �����϶� �˻�
//    if (SleepState == 2)//���� ��������϶�
//    {
//        if (player.GetMoveCheck())//�������ٸ�
//        {
//            MonsterAwake();//���
//        }
//        else//�������� �ʾҴٸ�
//        {
//            SleepState = 1;//�߰� ���� ���·� ���ư�
//            //MonsterRenderer.color = new Color(1, 0.92f, 0.016f, 1);
//            animator.SetInteger("SleepState", 1);//�߰� ���� ���·� �ִϸ��̼� ����
//            //Debug.Log("�߰� ���� ���·� ���ư��ϴ�.");
//        }
//    }//���� ���� ���� �� ���¿� �÷��̾ ������ �ٷ� ���