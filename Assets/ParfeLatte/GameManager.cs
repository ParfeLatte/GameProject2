using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<bool> accessLv = new List<bool>();//엑세스 레벨 저장
    public bool isTarget;//타겟을 확보했는지

    public Player player;//플레이어 스크립트

    public static bool IsPause { get => Time.timeScale <= 0.1f;
        set {
            Time.timeScale = value ? 1 : 0;
        }
    }

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
        return accessLv[gateLv-1];//문을 열기 위한 접근시도
    }

    public void GetCard(int level, bool isHave) {
        if(!accessLv[level-1] && isHave)
        {
            accessLv[level - 1] = true;//키를 소유하고있음
        }
    }

    public void Save() {
        //TODO: 여기서 저장할 데이터 전부다 넣어주기

        PlayerPrefs.Save();
    }

    public void Load() {
        
    }
}
