using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject TutorialCheckPanel;
    public GameObject SelectRundown;
    [SerializeField] private Button m_loadButton = null;
    [SerializeField] private GameObject m_optionUI = null;

    private void Awake() {
        if(m_loadButton == null)
            return;

        if(PlayerPrefs.HasKey("PlayerPosition"))
            m_loadButton.interactable = true;
    }

    public void OnClick_CheckTutorial()
    {
        TutorialCheckPanel.SetActive(true);
    }

    public void OnClick_SelectRundown()
    {
        SelectRundown.SetActive(true);
    }

    public void OnClick_NewGame() {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.ChangeSceneTo("Rundown_1");
    }

    public void OnClick_Load() {
        SceneController.Instance.ChangeSceneTo("Lab");
    }

    public void OnClick_Rundown1()
    {
        SceneController.Instance.ChangeSceneTo("Rundown_1");
    }

    public void OnClick_Rundown2()
    {
        SceneController.Instance.ChangeSceneTo("Rundown_2");
    }

    public void OnClick_Rundown3()
    {
        SceneController.Instance.ChangeSceneTo("Rundown_3");
    }

    public void OnClick_Rundown4()
    {
        SceneController.Instance.ChangeSceneTo("Rundown_4");
    }
    public void OnClick_Option() {
        if(m_optionUI == null)
            return;

        m_optionUI.SetActive(!m_optionUI.activeSelf);
    }

    public void OnClick_TutorialStart() {

        //PlayerPrefs.DeleteAll();
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
