using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using static Insomnia.Defines;

namespace Insomnia {
    public class ControllerBase : Interactable {
        [Header("ControllerBase: References")]
        [SerializeField] protected ControleeBase m_controlee = null;

        [Header("ControllerBase: Settings")]
        [SerializeField] protected object m_controlResult = 0;

        #region Properties
        public object ControlResult { get => m_controlResult; }

        #endregion

        public override void StandbyInteract() {
            if(m_canInteract == false)
                return;

            m_controlee.StandbyInteract();
        }

        public override void ReleaseInteract() {
            if(m_canInteract == false)
                return;

            m_controlee.ReleaseInteract();
        }

        public override bool OnInteractStart() {
            if(m_canInteract == false)
                return true;

            m_controlee.OnInteractStart(m_controlResult);
            return true;
        }

        public override void OnInteractEnd() {
            if(m_canInteract == false)
                return;

            m_controlee.OnInteractEnd();
        }
    }
}