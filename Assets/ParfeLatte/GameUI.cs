using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GoalText StageGoal;
    public TMP_Text MainGoal;
    public GameObject PausePanel;
    public GameObject AlertPanel;
    public Image alert;
    public float Alpha;
    public bool Minus;//참이면 마이너스

    private void Awake()
    {
        Alpha = 0.5f;
        if (MainGoal == null) return;
        MainGoal.text = StageGoal.Goal;
    }

    private void Update()
    {
        if (!AlertPanel.activeSelf) return;

        if (Alpha >= 0.5f)
        {
            Minus = true;
        }
        else if (Alpha <= 0.15f)
        {
            Minus = false;
        }
        if (Minus)
        {
            Alpha -= 0.1f * Time.deltaTime;
        }
        else if(!Minus)
        {
            Alpha += 0.1f * Time.deltaTime;
        }

        SetColor();
    }

    public void AlertOn()
    {
        Alpha = 0.5f;
        Minus = true;
        AlertPanel.SetActive(true);
    }
    public void AlertOff()
    {
        AlertPanel.SetActive(false);
    }

    public void SetColor()
    {
        alert.color = new Color(1, 0, 0, Alpha);
    }
    public void PausePanelOn()
    {
        PausePanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        PausePanel.SetActive(false);
    }
}
