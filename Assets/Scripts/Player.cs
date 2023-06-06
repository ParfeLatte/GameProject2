using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;//�ִ� �̵��ӵ�
    public float movetime;//�̵��� �ð� üũ
    public float stoptime;
    public bool isDash;//�뽬�ߴ��� üũ
    public bool isMove;//���������� üũ

    Rigidbody2D PlayerRigid;

    void Awake()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 curPos = transform.position;//������ġ
        Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//Ű�Է¿� ���� ���� ��ġ
        transform.position = curPos + nextPos;//������ġ�� ������ġ�� �������ν� �̵�

        if(h != 0)
        {
            isMove = true;
        }
        else if(h == 0)
        {
            isMove = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) { 
            isDash = true;//�뽬�ߴٸ� üũ
            Invoke("DashReset", 1f);
        }
        

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

    public void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
