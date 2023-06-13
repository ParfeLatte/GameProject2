using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Insomnia {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour {
        [Header("Interactable: Components")]
        private Collider _trigArea = null;  //Interactor와 충돌을 감지할 Trigger Collider
        private Interactor m_interactor = null;

        [Header("Interactable: Status")]
        [SerializeField] protected bool m_canInteract = true;

        [Header("Interactable: Settings")]
        public KeyCode m_interactKey = KeyCode.F;

        [Header("Interactable: Events")]
        public UnityEvent<Interactable> onInteractStart = null;
        public UnityEvent<Interactable> onInteractEnd = null;

        #region Properties
        public KeyCode InteractKey { get => m_interactKey; }
        public bool CanInteract { get => m_canInteract; }

        protected Interactor User { get => m_interactor; }
        
        #endregion

        protected virtual void Awake() {
            _trigArea = GetComponent<Collider>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.gameObject.TryGetComponent(out Interactor interactor)) {
                m_interactor = interactor;
                m_interactor.StandbyInteract(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if(m_interactor == null)
                return;

            if(m_interactor.gameObject != collision.gameObject)
                return;

            m_interactor.ReleaseInteract(this);
        }

        /// <summary>
        /// 플레이어가 트리거에 들어왔을 때 실행되는 함수
        /// </summary>
        public virtual void StandbyInteract() { }

        /// <summary>
        /// 플레이어가 트리거에서 탈출할 때 실행되는 함수
        /// </summary>
        public virtual void ReleaseInteract() { }

        /// <summary>
        /// 플레이어가 상호작용을 시작했을 때 실행되는 함수.
        /// <seealso cref="m_canInteract"/>가 true일 때 실행되며, false일 때는 기능을 수행하지 않고 종료한다.
        /// </summary>
        /// <returns>isOneShot : return true if Interaction should be finished Immediately. else false.</returns>
        public virtual bool OnInteractStart() { return true; }

        /// <summary>
        /// 플레이어가 상호작용을 종료했을 때 실행되는 함수. 
        /// 만약 종료 과정 없이 시작하자마자 true를 return할 경우 override 불필요.
        /// </summary>
        public virtual void OnInteractEnd() { }

        public virtual void InteractConditionSolved(object condition) { m_canInteract = true; }
    }
}
