using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class InputListener : MonoBehaviour {
        [SerializeField] protected ushort _priority = 0;
        [SerializeField] private bool _isOverridable = false;
        private bool _isEnabled = false;
        public ushort Priority { get => _priority; }
        public bool IsOverridable { get => _isOverridable; }

        private InputActor[] _actors = null;
        private InputManager _manager = null;

        #region Unity Event Functions
        private void Awake() {
            _actors = GetComponentsInChildren<InputActor>();
        }

        private void Start() {
            InputManager.Instance.AddListener(this);
        }

        private void OnEnable() {
            AfterOnEnableCalled();
        }

        protected virtual void AfterOnEnableCalled() { }

        private void OnDisable() {
            InputManager manager= InputManager.Instance;
            if(manager != null)
                manager.RemoveListener(this);
            AfterOnDisableCalled();
        }

        protected virtual void AfterOnDisableCalled() { }

        private void OnDestroy() { OnDisable(); }

        public virtual void OnUpdate() {
            for(int i = 0; i < _actors.Length; i++) {
                _actors[i].KeyCheck();
            }
        }

        public void EnableListener() {
            if(_isEnabled)
                return;

            for(int i = 0; i < _actors.Length; i++) {
                _actors[i].ListenerEnabled();
            }
            _isEnabled = true;
        }

        public void DisableListener() {
            if(_isEnabled == false)
                return;

            for(int i = 0; i < _actors.Length; i++) {
                _actors[i].ListenerDisabled();
            }
            _isEnabled = false;
        }
        #endregion
    }
}
