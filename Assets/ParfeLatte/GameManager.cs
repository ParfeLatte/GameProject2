using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<bool> accessLv = new List<bool>();//엑세스 레벨 저장

    public Player player;//플레이어 스크립트
    void Update()
    {
        
    }

    public bool CheckGateOpen(int gateLv)
    {
        return accessLv[gateLv-1];//문을 열기 위한 접근시도
    }

    public void GetCard(int level, bool isHave) {
        if(!accessLv[level-1] && isHave)
        {
            accessLv[level - 1] = true;//키를 소유하고있음
        }
    }
}
