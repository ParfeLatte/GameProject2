using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeUI : MonoBehaviour {
    [SerializeField] private GameObject m_optionUI = null;
    private void OnEnable() {
        Time.timeScale = 0f;
    }

    private void OnDisable() {
        m_optionUI.SetActive(false);
        OnClick_Resume();
    }

    public void OnClick_Resume() {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void OnClick_Option() {
        if(m_optionUI == null)
            return;

        m_optionUI.SetActive(!m_optionUI.activeSelf);
    }

    public void OnClick_Quit() {
        Application.Quit();
    }
}