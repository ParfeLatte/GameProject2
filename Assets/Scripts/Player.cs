using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;//최대 이동속도
    public float movetime;//이동한 시간 체크
    public float stoptime;
    public bool isDash;//대쉬했는지 체크
    public bool isMove;//움직였는지 체크

    Rigidbody2D PlayerRigid;

    void Awake()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 curPos = transform.position;//현재위치
        Vector3 nextPos = new Vector3(h, 0, 0) * Speed * Time.deltaTime;//키입력에 따른 다음 위치
        transform.position = curPos + nextPos;//현재위치와 다음위치를 더함으로써 이동

        if(h != 0)
        {
            isMove = true;
        }
        else if(h == 0)
        {
            isMove = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) { 
            isDash = true;//대쉬했다를 체크
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
    }//대쉬했음을 체크해서 보내줌

    private void DashReset()
    {
        isDash = false;
    }//대쉬이후 초기화

    public void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
