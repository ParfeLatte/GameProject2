using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class EscapeInputActor : KeyboardButtonActor {
        [SerializeField] private GameObject m_escapeUI = null;

        public void OnKeyDown_Escape() {
            if(m_escapeUI == null)
                return;

            m_escapeUI.SetActive(!m_escapeUI.activeSelf);
        }
    }
}

