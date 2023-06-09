using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Insomnia {
    public class VolumeUI : MonoBehaviour {
        [SerializeField] private Slider m_master = null;
        [SerializeField] private Slider m_bgm = null;
        [SerializeField] private Slider m_sfx = null;

        private void Start() {
            VolumeController controller = VolumeController.Instance;
            if(controller == null)
                return;

            if(m_master == null || m_bgm == null || m_sfx == null)
                return;

            m_master.value = controller.Volume_Master;
            m_bgm.value = controller.Volume_BGM;
            m_sfx.value = controller.Volume_SFX;
        }

        public void OnValueChanged_Master(Slider slider) {
            VolumeController controller = VolumeController.Instance;
            if(controller == null)
                return;

            controller.Volume_Master = Mathf.Clamp01(slider.value);
        }

        public void OnValueChanged_BGM(Slider slider) {
            VolumeController controller = VolumeController.Instance;
            if(controller == null)
                return;

            controller.Volume_BGM = Mathf.Clamp01(slider.value);
        }

        public void OnValueChanged_SFX(Slider slider) {
            VolumeController controller = VolumeController.Instance;
            if(controller == null)
                return;

           controller.Volume_SFX = Mathf.Clamp01(slider.value);
        }
    }
}