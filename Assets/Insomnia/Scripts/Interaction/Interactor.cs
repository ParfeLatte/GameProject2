using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Insomnia {
    public class Interactor : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private KeyCode _interactKey = KeyCode.F;
        [SerializeField] private bool _isUsingInputActor = false;
        private Interactable _curInteract = null;

        [Header("Events")]
        public UnityEvent onInteractStandby = null;
        public UnityEvent onInteractRelease = null;
        public UnityEvent onInteractStart = null;
        public UnityEvent onInteractEnd = null;

        private void Update() {
            if(_isUsingInputActor)
                return;

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
            onInteractStandby?.Invoke();
        }

        public virtual void ReleaseInteract(Interactable interactable) {
            if(interactable == null)
                return;

            if(_curInteract != interactable)
                return;

            _curInteract.ReleaseInteract();
            _curInteract = null;
            onInteractRelease?.Invoke();
        }

        public virtual void OnInteractStart() {
            if(_curInteract == null)
                return;

            _curInteract.OnInteractStart();
            onInteractStart?.Invoke();
        }

        public virtual void OnInteractEnd() {
            if(_curInteract == null)
                return;

            _curInteract.OnInteractEnd();
            onInteractEnd?.Invoke();
        }
    }
}