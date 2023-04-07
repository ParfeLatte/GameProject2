using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject Player;
    public Player player;
    public GameObject Mob;

    public GameObject spawnEnemy(Transform SpawnPos)
    {
        GameObject enemy = Instantiate(Mob, SpawnPos);
        Monster mobLogic = enemy.GetComponent<Monster>();
        mobLogic.Player = Player;
        mobLogic.player = player;
        return enemy;
    }
}
