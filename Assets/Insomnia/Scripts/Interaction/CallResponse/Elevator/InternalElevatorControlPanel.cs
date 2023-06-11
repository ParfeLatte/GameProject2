using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class InternalElevatorControlPanel : ControllerBase {
        [Header("ControlPanel: External References")]
        [SerializeField] protected GameObject m_tutorialUI = null;

        [Header("ElevatorControlPanel: Settings")]
        [SerializeField] private int m_targetDirection = 1;

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

            Elevator elevator = m_reactor as Elevator;
            if(elevator == null)
                return true;

            if(elevator.IsReacting)
                return true;

            bool success = m_reactor.OnInteractStart(elevator.CurrentFloor + m_targetDirection);
            if(success) {
                //m_tutorialUI?.SetActive(false);
            }
                
            return true;
        }

        public override void OnInteractEnd() {
            if(m_canInteract == false)
                return;

            bool success = m_reactor.OnInteractEnd();
            if(success) {

            }
        }
    }
}

