using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject PausePanel;
    

    public void PausePanelOn()
    {
        PausePanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        PausePanel.SetActive(false);
    }
}
