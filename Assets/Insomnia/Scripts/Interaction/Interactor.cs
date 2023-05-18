using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class Interactor : MonoBehaviour {
        private Interactable _curInteract = null;
        [SerializeField] private KeyCode _interactKey = KeyCode.F;

        private void Update() {
            if(Input.GetKeyDown(_interactKey)) {
                OnInteractStart();
            }
        }

        public virtual void StandbyInteract(Interactable interactable) {
            if(interactable == null)
                return;

            if(_curInteract == interactable)
                return;

            _curInteract = interactable;
            _curInteract.StandbyInteract();
        }

        public virtual void ReleaseInteract(Interactable interactable) {
            if(interactable == null)
                return;

            if(_curInteract != interactable)
                return;

            _curInteract.ReleaseInteract();
            _curInteract = null;
        }

        public virtual void OnInteractStart() {
            if(_curInteract == null)
                return;

            _curInteract.OnInteractStart();
        }

        public virtual void OnInteractEnd() {
            if(_curInteract == null)
                return;

            _curInteract.OnInteractEnd();
        }
    }
}