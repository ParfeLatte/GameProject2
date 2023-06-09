using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject TutorialCheckPanel;
    [SerializeField] private Button m_loadButton = null;
    [SerializeField] private GameObject m_optionUI = null;

    private void Awake() {
        if(m_loadButton == null)
            return;

        if(PlayerPrefs.HasKey("PlayerPosition"))
            m_loadButton.interactable = true;
    }

    public void OnClick_NewGame() {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.ChangeSceneTo("Lab");
    }

    public void OnClick_Load() {
        SceneController.Instance.ChangeSceneTo("Lab");
    }

    public void OnClick_Option() {
        if(m_optionUI == null)
            return;

        m_optionUI.SetActive(!m_optionUI.activeSelf);
    }

    public void OnClick_TutorialStart() {
        SceneController.Instance.ChangeSceneTo("Tutorial");
    }

    public void OnClick_Exit() {
        Application.Quit();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    
}
