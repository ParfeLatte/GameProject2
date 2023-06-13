using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia{
	public class RecvControllerBase : ReceiverBase {
        [Header("RecvControllerBase: External References")]
        [SerializeField] protected ControleeBase m_controlee = null;

        [Header("RecvControllerBase: Settings")]
        [SerializeField] protected object m_controlResult = 0;

        public object ControlResult { get => m_controlResult; }

        #region ReceiverBase Functions
        protected override void Awake() {
            base.Awake();
        }

        public override void InteractConditionSolved(object condition) {
            base.InteractConditionSolved(condition);
        }

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

        public override void OnInteractStartSuccess() {
            m_controlee.OnInteractStart(m_controlResult);
        }

        public override void OnInteractEnd() {
            if(m_canInteract == false)
                return;

            onInteractEnd?.Invoke(this);
            onInteractEnd = null;
            m_controlee.OnInteractEnd();
        }

        #endregion
    }
}
