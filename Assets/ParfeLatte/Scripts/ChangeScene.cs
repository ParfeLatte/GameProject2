using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    
    public void changeToGame()
    {
        SceneController.Instance.ChangeSceneTo("Lab");
    }

    public void changeToTutorial()
    {
        SceneController.Instance.ChangeSceneTo("Tutorial");
    }

    public void changeToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            changeToGame();
        }
    }
}
