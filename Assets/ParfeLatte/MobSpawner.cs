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
        GameObject enemy = Instantiate(Mob, SpawnPos);//���� ����(������Ʈ Ǯ������ ��ü ����)
        Monster mobLogic = enemy.GetComponent<Monster>();//���� ������Ʈ �ҷ��ͼ�
        mobLogic.Player = Player;//�÷��̾� ������Ʈ  ����
        mobLogic.player = player;//�÷��̾� ��ũ��Ʈ ���� 
        return enemy;//����
    }
}
