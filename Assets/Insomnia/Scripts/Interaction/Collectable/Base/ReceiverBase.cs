using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia{
	public class ReceiverBase : Interactable {
        [Header("Receiver: Settings")]
        [SerializeField] protected CollectableType m_collectType = CollectableType.None;

        #region Interactable Functions
        protected override void Awake() {
            base.Awake();
        }

        public override void InteractConditionSolved(object condition) {
            base.InteractConditionSolved(condition);
        }
        public override void StandbyInteract() {

        }

        public override void ReleaseInteract() {

        }

        public sealed override bool OnInteractStart() {
            if(m_canInteract == false)
                return true;

            Collector collector = User.GetComponent<Collector>();
            if(collector == null)
                return true;

            if(collector.GetItem(m_collectType) == false)
                return true;

            onInteractStart?.Invoke(this);
            onInteractStart = null;
            OnInteractStartSuccess();
            return true;
        }

        public override void OnInteractEnd() {
            onInteractEnd?.Invoke(this);
            onInteractEnd = null;
        }

        public virtual void OnInteractStartSuccess() { }

        #endregion
    }
}
