using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Insomnia {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour {
        [Header("Interactable: Components")]
        private Collider _trigArea = null;  //Interactor�� �浹�� ������ Trigger Collider
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
        /// �÷��̾ Ʈ���ſ� ������ �� ����Ǵ� �Լ�
        /// </summary>
        public virtual void StandbyInteract() { }

        /// <summary>
        /// �÷��̾ Ʈ���ſ��� Ż���� �� ����Ǵ� �Լ�
        /// </summary>
        public virtual void ReleaseInteract() { }

        /// <summary>
        /// �÷��̾ ��ȣ�ۿ��� �������� �� ����Ǵ� �Լ�.
        /// <seealso cref="m_canInteract"/>�� true�� �� ����Ǹ�, false�� ���� ����� �������� �ʰ� �����Ѵ�.
        /// </summary>
        /// <returns>isOneShot : return true if Interaction should be finished Immediately. else false.</returns>
        public virtual bool OnInteractStart() { return true; }

        /// <summary>
        /// �÷��̾ ��ȣ�ۿ��� �������� �� ����Ǵ� �Լ�. 
        /// ���� ���� ���� ���� �������ڸ��� true�� return�� ��� override ���ʿ�.
        /// </summary>
        public virtual void OnInteractEnd() { }

        public virtual void InteractConditionSolved(object condition) { m_canInteract = true; }
    }
}
