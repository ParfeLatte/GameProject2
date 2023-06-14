using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Insomnia.Defines;

namespace Insomnia{
	public class CollectableBase : Interactable{
        [Space]
        [Header("CollectableBase: Status")]
        [SerializeField] protected bool m_canCollect = true;

        [Header("CollectableBase: Settings")]
        [SerializeField] private CollectableData m_itemData;

        [Header("CollectableBase: Events")]
        [SerializeField] protected UnityEvent<Interactable> onCollectFailed = null;
        [SerializeField] protected UnityEvent<Interactable> onCollectSuccess = null;

        #region Properties
        public CollectableData Data { get => m_itemData; }

        #endregion

        #region Unity Event Functions
        #endregion

        #region Interactable Functions
        protected override void Awake() {
            base.Awake();
            m_itemData.BelongedTo = this;
        }

        public override void InteractConditionSolved(object condition) {
            m_canCollect = true;
        }

        public override void StandbyInteract() {
            base.StandbyInteract();
        }

        public override void ReleaseInteract() {
            base.ReleaseInteract();
        }

        public override bool OnInteractStart() {
            if(m_canCollect == false) {
                onCollectFailed?.Invoke(this);
                onCollectFailed = null;
                return true;
            }

            if(User == null)
                return true;

            Collector collector = User.GetComponent<Collector>();
            if(collector == null)
                return true;

            collector.AddItem(this);
            onCollectSuccess?.Invoke(this);
            onCollectSuccess = null;
            gameObject.SetActive(false);
            return true;
        }

        #endregion
    }
}
