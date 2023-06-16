using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insomnia{
	public class RundownSaver : MonoBehaviour {
        [Header("RundownSaver: Status")]
        [SerializeField] private string m_saveRundownName = "";
        private void Awake() {
            Scene curScene = SceneManager.GetActiveScene();
            m_saveRundownName = curScene.name;
        }

        public void OnRundownClear() { 
            PlayerPrefs.SetInt($"{m_saveRundownName}_Cleared".ToUpper(), 1);
            PlayerPrefs.Save();
            SceneController.Instance.ChangeSceneToSuccess();
        }
    }
}
