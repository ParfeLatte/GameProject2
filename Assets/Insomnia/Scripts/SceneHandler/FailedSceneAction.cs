using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insomnia {
    public class FailedSceneAction : MonoBehaviour {
        public void OnClick_MainMenu() {
            if(SceneController.Instance.IsLoading) 
                return;

            SceneController.LoadSceneCompleted.Enqueue(() => GameManager.Instance.RemoveData());
            SceneController.Instance.ChangeSceneTo("Main");
        }

        public void OnClick_Restart() {
            if(SceneController.Instance.IsLoading)
                return;

            SceneController.LoadSceneCompleted.Enqueue(() => GameManager.Instance.LoadData());
            SceneController.Instance.ChangeSceneTo("Lab");
        }
    }
}

