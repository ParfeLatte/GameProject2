using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<bool> accessLv = new List<bool>();//������ ���� ����
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void GetCard(int level, bool isHave) {
        if(!accessLv[level-1] && isHave)
        {
            accessLv[level - 1] = true;//Ű�� �����ϰ�����
        }
    }
}
