using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<bool> accessLv = new List<bool>();//������ ���� ����
    public bool isTarget;//Ÿ���� Ȯ���ߴ���

    public Player player;//�÷��̾� ��ũ��Ʈ
    void Update()
    {
        
    }

    private void Awake()
    {
        isTarget = false;
    }

    public void GetTarget()
    {
        isTarget = true;
    }
    public bool Target()
    {
        return isTarget;
    }
    public bool CheckGateOpen(int gateLv)
    {
        return accessLv[gateLv-1];//���� ���� ���� ���ٽõ�
    }

    public void GetCard(int level, bool isHave) {
        if(!accessLv[level-1] && isHave)
        {
            accessLv[level - 1] = true;//Ű�� �����ϰ�����
        }
    }
}
