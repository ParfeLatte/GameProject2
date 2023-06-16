using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insomnia {
    public class FailedSceneAction : MortalSingleton<FailedSceneAction> {
        public string PrevSceneName = "";

        private void Awake() {
            PrevSceneName = PlayerPrefs.GetString("Failed".ToUpper());
        }

        public void OnClick_MainMenu() {
            if(SceneController.Instance.IsLoading) 
                return;

            SceneController.Instance.ChangeSceneTo("Main");
        }

        public void OnClick_Restart() {
            if(SceneController.Instance.IsLoading)
                return;

            if(PrevSceneName == string.Empty)
                PrevSceneName = "Main";
            else 
                PlayerPrefs.DeleteKey("Failed".ToUpper());

            SceneController.Instance.ChangeSceneTo(PrevSceneName);
        }
    }
}

