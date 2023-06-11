using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Insomnia {
    public class Interactor : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private bool _isUsingInputActor = false;
        private List<Interactable> _curInteracts = new List<Interactable>();

        [Header("Events")]
        public UnityEvent<Interactable> onInteractStandby = null;
        public UnityEvent<Interactable> onInteractRelease = null;
        public UnityEvent<Interactable> onInteractStart = null;
        public UnityEvent<Interactable> onInteractEnd = null;

        private void Update() {
            if(_isUsingInputActor)
                return;

            if(_curInteracts == null)
                return;

            if(_curInteracts.Count < 0)
                return;

            for(int i = 0; i < _curInteracts.Count; i++) {
                if(Input.GetKeyDown(_curInteracts[i].InteractKey)) {
                    OnInteractStart(_curInteracts[i]);
                    break;
                }
            }
        }

        public virtual void StandbyInteract(Interactable interactable) {
            if(interactable == null)
                return;

            if(_curInteracts.Contains(interactable))
                return;

            _curInteracts.Add(interactable);
            interactable.StandbyInteract();
            interactable.onInteractEnd.AddListener(OnInteractEnd);
            onInteractStandby?.Invoke(interactable);
        }

        public virtual void ReleaseInteract(Interactable interactable) {
            if(interactable == null)
                return;

            if(_curInteracts.Contains(interactable) == false)
                return;

            interactable.onInteractEnd.RemoveListener(OnInteractEnd);
            interactable.ReleaseInteract();
            onInteractRelease?.Invoke(interactable);
            _curInteracts.Remove(interactable);
        }

        public virtual void OnInteractStart(Interactable interactable) {
            if(_curInteracts == null)
                return;

            onInteractStart?.Invoke(interactable);
            bool isOneshot = interactable.OnInteractStart();

            if(isOneshot)
                OnInteractEnd(interactable);
        }

        public virtual void OnInteractEnd(Interactable interactable) {
            if(_curInteracts == null)
                return;

            interactable.OnInteractEnd();
            onInteractEnd?.Invoke(interactable);
        }
    }
}