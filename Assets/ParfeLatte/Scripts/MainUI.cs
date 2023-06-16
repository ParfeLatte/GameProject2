using Insomnia;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Insomnia.BGM_Speaker;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject m_selectRundown = null;
    [SerializeField] private GameObject m_optionUI = null;

    public void OnClick_SelectRundown() {
        m_selectRundown.SetActive(true);
    }

    public void OnClick_Rundown(int index) {
        BGM_Speaker.Instance.Stop();
        SceneController.Instance.ChangeSceneTo($"RunDown_{index}");
    }

    public void OnClick_Option() {
        if(m_optionUI == null)
            return;

        m_optionUI.SetActive(!m_optionUI.activeSelf);
    }

    public void OnClick_Reset() {
        for(int i = 0; i < 5; i++) {
            PlayerPrefs.SetInt($"RunDown_{i}_Cleared".ToUpper(), 0);
        }
    }

    public void OnClick_Exit() {
        Application.Quit();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    
}
