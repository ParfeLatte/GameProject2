using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour {
        private Collider _trigArea = null;  //Interactor�� �浹�� ������ Trigger Collider
        private Interactor _interactor = null;

        protected virtual void Awake() {
            _trigArea = GetComponent<Collider>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.gameObject.TryGetComponent(out Interactor interactor)) {
                _interactor = interactor;
                _interactor.StandbyInteract(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if(_interactor.gameObject != collision.gameObject)
                return;

            _interactor.ReleaseInteract(this);
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
        /// �÷��̾ ��ȣ�ۿ��� �������� �� ����Ǵ� �Լ�
        /// </summary>
        public virtual void OnInteractStart() { }

        /// <summary>
        /// �÷��̾ ��ȣ�ۿ��� �������� �� ����Ǵ� �Լ�
        /// </summary>
        public virtual void OnInteractEnd() { }
    }
}
