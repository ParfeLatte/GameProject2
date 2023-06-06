using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Insomnia {
    public class ProgressBar : MonoBehaviour {
        [SerializeField]private Slider m_progressSlider = null;
        private SceneController controller = null;
        

        private void Awake() {
            m_progressSlider = GetComponentInChildren<Slider>();
        }

        private void Start() {
            controller = SceneController.Instance;
            if(controller == null)
                gameObject.SetActive(false);
        }

        private void Update() {
            if(controller == null)
                return;

            m_progressSlider.value = controller.Progress;
        }
    }
}