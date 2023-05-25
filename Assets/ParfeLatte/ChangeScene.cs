using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void changeToGame()
    {
        SceneManager.LoadScene("Lab");
    }

    public void changeToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void changeToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}
