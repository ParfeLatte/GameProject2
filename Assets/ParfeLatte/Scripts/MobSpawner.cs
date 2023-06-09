using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject Player;
    public ObjectManager Obj;
    public Player player;

    public GameObject spawnEnemy(Transform SpawnPos)
    {
        GameObject enemy = Obj.PullMob();//몬스터 생성(오브젝트 풀링으로 대체 예정)
        Monster mobLogic = enemy.GetComponent<Monster>();//몬스터 컴포넌트 불러와서
        enemy.transform.position = SpawnPos.position;
        mobLogic.Player = Player;//플레이어 오브젝트  지정
        mobLogic.player = player;//플레이어 스크립트 지정 
        return enemy;//리턴
    }
}
