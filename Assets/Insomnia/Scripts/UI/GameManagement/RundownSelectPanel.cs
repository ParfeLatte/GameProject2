using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Insomnia{
	public class RundownSelectPanel : MonoBehaviour {
        [Header("RundownSelect: External References")]
        [SerializeField] private Button[] m_rundownButtons = null;

        private void Awake() {
            if(m_rundownButtons == null)
                return;

            if(m_rundownButtons.Length == 0)
                return;

            Refresh();
        }

        public void Refresh() {
            m_rundownButtons[0].interactable = true;

            for(int i = 1; i < 5; i++) {
                if(PlayerPrefs.GetInt($"{m_rundownButtons[i - 1].name}_Cleared".ToUpper()) == 1)
                    m_rundownButtons[i].interactable = true;
                else
                    m_rundownButtons[i].interactable = false;
            }
        }
    }
}
