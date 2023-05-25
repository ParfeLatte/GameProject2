using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject TutorialCheckPanel;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialPanel()
    {
        TutorialCheckPanel.SetActive(true);
    }
    
    public void PanelOff()
    {
        TutorialCheckPanel.SetActive(false);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    
}
