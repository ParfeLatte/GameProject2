using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public GameUI UI;
    public bool isPause;


    private void Awake()
    {
        isPause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        UI.PausePanelOn();
        Time.timeScale = 0f;
        isPause = true;
    }
    public void Resume()
    {
        UI.ClosePausePanel();
        Time.timeScale = 1f;
        isPause = false;
    }
}
