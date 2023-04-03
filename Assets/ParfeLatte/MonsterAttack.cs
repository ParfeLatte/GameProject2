using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public Monster monster;//몬스터(공격의 주체)
    public Vector3 curPos;//현재 공격범위 위치

    void Start()
    {
        curPos = transform.localPosition;//부모 오브젝트를 기준으로 
    }

    void Update()
    {
        if (monster.Dir == 1)
        {
            this.transform.localPosition = new Vector3(curPos.x, curPos.y, curPos.z);//공격 범위 위치 조절(플레이어 오른쪽)
        }
        if (monster.Dir == -1)
        {
            this.transform.localPosition = new Vector3(curPos.x * -1, curPos.y, curPos.z);//공격 범위 위치 조절(플레이어 왼쪽)
        }
    }
}
