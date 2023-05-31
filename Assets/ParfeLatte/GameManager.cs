using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

public class GameManager : Singleton<GameManager>, IDataIO
{
    struct GameManagerSaveData {
        public List<bool> accessLv;
        public bool isTarget;
    }

    public List<bool> accessLv = new List<bool>();//������ ���� ����
    public bool isTarget;//Ÿ���� Ȯ���ߴ���

    public Player player;//�÷��̾� ��ũ��Ʈ

    public static bool IsPause { get => Time.timeScale <= 0.1f;
        set {
            Time.timeScale = value ? 1 : 0;
        }
    }

    void Update()
    {
        
    }

    protected override void Awake()
    {
        base.Awake();
        isTarget = false;
    }

    private void OnApplicationQuit() {
        RemoveData();
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

    public void Save() {
        //TODO: ���⼭ ������ ������ ���δ� �־��ֱ�
        if(player != null)
            player.SaveData();
        SaveData();
        PlayerPrefs.Save();
    }

    public void Load() {
        LoadData();
        if(player != null)
            player.LoadData();
    }

    public void AddPlayer(Player player) {
        if(player.isDead)
            return;

        this.player = player;
        player.LoadData();
    }

    public void SaveData() {
        GameManagerSaveData saveData = new GameManagerSaveData(){ isTarget = isTarget, accessLv = accessLv };
        string jsonData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("GameManager", jsonData);
        PlayerPrefs.Save();
    }

    public void LoadData() {
        if(PlayerPrefs.HasKey("GameManager") == false)
            return;

        string jsonData = PlayerPrefs.GetString("GameManager");
        if(jsonData == string.Empty || jsonData == "")
            return;

        GameManagerSaveData prevData = JsonUtility.FromJson<GameManagerSaveData>(jsonData);
        if(prevData.Equals(default(GameManagerSaveData)))
            return;

        accessLv = prevData.accessLv;
        isTarget = prevData.isTarget;
    }

    public void RemoveData() {
        //PlayerPrefs.DeleteKey("GameManager");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
