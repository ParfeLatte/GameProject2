using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insomnia {
    public abstract class SceneChangeEffect : MonoBehaviour {
        protected static SceneController m_controller = null;
        protected bool m_curEffectFinished = true;
        [SerializeField] private bool m_autoRegister = true;
        [SerializeField] private bool m_isTemporal = true;

        public bool EffectFinished { get {
                if(m_curEffectFinished) {
                    m_curEffectFinished = false;
                    return true;
                }
                return false;
            } 
        }
        public bool IsTemporal { get => m_isTemporal; }

        #region Unity Event Functions
        private void Awake() {
            Awake_Child();
            Init();
        }

        private void Init() {
            m_curEffectFinished = false;
            m_controller = null;

            Init_Child();
        }

        protected virtual void Awake_Child() { }
        protected virtual void Init_Child() { }

        private void Start() {
            m_controller = SceneController.Instance;
            if(m_controller == null) {
                gameObject.SetActive(false);
                return;
            }

            if(m_autoRegister) {
                if(m_controller.AddSceneChangeEffect(this) == false) {
                    gameObject.SetActive(false);
                    return;
                }
            }

            Start_Child();
        }

        protected virtual void Start_Child() { }

        #endregion

        #region Concrete Functions
        public void RegisterEffect() {
            if(m_autoRegister)
                return;
            if(m_controller == null)
                return;

            Init();
            Init_Child();
            m_controller.AddSceneChangeEffect(this);
        }

        #endregion

        #region Abstract Functions
        public abstract void StartEffect();

        public abstract void FinishEffect();

        protected abstract IEnumerator CoEffect(object targetValue);

        #endregion
    }
}