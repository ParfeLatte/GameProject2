using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour {
        private Collider _trigArea = null;  //Interactor와 충돌을 감지할 Trigger Collider
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
        /// 플레이어가 트리거에 들어왔을 때 실행되는 함수
        /// </summary>
        public virtual void StandbyInteract() { }

        /// <summary>
        /// 플레이어가 트리거에서 탈출할 때 실행되는 함수
        /// </summary>
        public virtual void ReleaseInteract() { }

        /// <summary>
        /// 플레이어가 상호작용을 시작했을 때 실행되는 함수
        /// </summary>
        public virtual void OnInteractStart() { }

        /// <summary>
        /// 플레이어가 상호작용을 종료했을 때 실행되는 함수
        /// </summary>
        public virtual void OnInteractEnd() { }
    }
}
