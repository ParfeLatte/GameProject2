using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<bool> accessLv = new List<bool>();//������ ���� ����

    public Player player;
    void Update()
    {
        
    }

    public bool CheckGateOpen(int gateLv)
    {
        return accessLv[gateLv-1];
    }

    public void GetCard(int level, bool isHave) {
        if(!accessLv[level-1] && isHave)
        {
            accessLv[level - 1] = true;//Ű�� �����ϰ�����
        }
    }
}
