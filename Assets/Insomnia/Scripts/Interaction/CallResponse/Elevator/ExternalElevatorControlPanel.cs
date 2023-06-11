using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class ExternalElevatorControlPanel : ControllerBase {
        [Header("ControlPanel: External References")]
        [SerializeField] protected GameObject m_tutorialUI = null;

        [Header("ControlPanel: Settings")]
        [SerializeField] private int m_targetFloor = 0;

        public override void StandbyInteract() {
            if(m_canInteract == false)
                return;

            m_reactor.StandbyInteract();
            //m_tutorialUI?.SetActive(true);
        }

        public override void ReleaseInteract() {
            if(m_canInteract == false)
                return;

            m_reactor.ReleaseInteract();
            //m_tutorialUI?.SetActive(false);
        }

        public override bool OnInteractStart() {
            if(m_canInteract == false)
                return true;

            m_reactor.OnInteractStart(m_targetFloor);
            //m_tutorialUI?.SetActive(false);
            return true;
        }

        public override void OnInteractEnd() {
            if(m_canInteract == false)
                return;

            m_reactor.OnInteractEnd();
        }
    }
}