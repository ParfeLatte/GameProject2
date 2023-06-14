using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Player player;//플레이어(공격의 주체)
    public Vector3 curPos;//현재 공격범위의 위치

    private float Damage;//가하는 데미지
    private bool isAttack;//공격했냐

    public List<Collider2D> TargetList = new List<Collider2D>();//공격 대상들을 담아놓을 리스트

    // Start is called before the first frame update
    void Start()
    {
        curPos = transform.localPosition;//부모 오브젝트를 기준으로 함
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if(player.Dir == 1) {
            this.transform.localPosition = new Vector3(curPos.x, curPos.y, curPos.z);//공격 범위 위치 조절(플레이어 오른쪽)
        }
        if (player.Dir == -1)
        {
            this.transform.localPosition = new Vector3(curPos.x * -1, curPos.y, curPos.z);//공격 범위 위치 조절(플레이어 왼쪽)
        }
    }

    public void GetAttack(float damage)
    {
        Damage = damage;//공격할 데미지를 정함
        if (!isAttack)//이미 공격중일때를 대비한 예외처리
        {
            isAttack = true;//공격중이다!
            Invoke("AttackMonster", 0.2f);//공격 실행(데미지 넣는 처리)
        }
    }

    private void AttackMonster()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            Monster enemy = TargetList[i].GetComponent<Monster>();//대상에게서 몬스터 스크립트를  받아옴
            enemy.damaged(Damage);//공격받을때 부르는 함수
        }
        isAttack = false;
    }//공격 대상으로 들어왔있는 모든 몬스터에게 데미지가 들어감

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.tag == "enemy" && !TargetList.Contains(col))
        {
            if (TargetList.Count < 5)
            {
                TargetList.Add(col);//몬스터가 공격범위 내에 들어오면 공격대상에 추가
            }
        }
        else
        {

        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (TargetList.Contains(col))
        {
            TargetList.Remove(col);//범위를 벗어나면 리스트에 있던 몬스터를 제거함(공격대상X)
        }
        else
        {

        }
    }
}


//private void OnTriggerStay2D(Collider2D col)
//{
//    //if (col.tag == "enemy")
//    //{
//    //    if (isAttack)
//    //    {
//    //        Monster enemy = col.GetComponent<Monster>();
//    //        enemy.damaged(Damage);
//    //        isAttack = false;
//    //    }
//    //    //공격처리!
//    //}
//}
