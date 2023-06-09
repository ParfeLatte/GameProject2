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
            _curInteract.onInteractEnd.AddListener(OnInteractEnd);
            onInteractStandby?.Invoke();
        }

        public virtual void ReleaseInteract(Interactable interactable) {
            if(interactable == null)
                return;

            if(_curInteract != interactable)
                return;

            _curInteract.onInteractEnd.RemoveListener(OnInteractEnd);
            _curInteract.ReleaseInteract();
            _curInteract = null;
            onInteractRelease?.Invoke();
        }

        public virtual void OnInteractStart() {
            if(_curInteract == null)
                return;

            onInteractStart?.Invoke();
            bool isOneshot = _curInteract.OnInteractStart();

            if(isOneshot)
                OnInteractEnd();
        }

        public virtual void OnInteractEnd() {
            if(_curInteract == null)
                return;

            onInteractEnd?.Invoke();
            //_curInteract.OnInteractEnd();
        }
    }
}