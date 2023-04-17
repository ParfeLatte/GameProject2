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
        GameObject enemy = Obj.PullMob();//���� ����(������Ʈ Ǯ������ ��ü ����)
        Monster mobLogic = enemy.GetComponent<Monster>();//���� ������Ʈ �ҷ��ͼ�
        enemy.transform.position = SpawnPos.position;
        mobLogic.Player = Player;//�÷��̾� ������Ʈ  ����
        mobLogic.player = player;//�÷��̾� ��ũ��Ʈ ���� 
        return enemy;//����
    }
}
