using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Insomnia {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(ScannerGizmo))]
    public class Scanner : MonoBehaviour {
        [Header("Components")]
        [SerializeField] private ScanActionSO m_action = null;
        [SerializeField]private Canvas m_scanCanvas = null;
        [SerializeField]private Slider m_scanSlider = null;
        private ScannerGizmo m_gizmo = null;
        private Collider2D m_scanTrigger = null;

        [Header("Settings")]
        [SerializeField] private float m_scanIncreaseAmount = 0f;
        [SerializeField] private UnityEvent onScanCompleted = null;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
#endif
        private bool m_isTriggered = false;

        [Header("Stat")]
        [SerializeField] private float m_scannedAmount = 0f;

        public bool IsTriggered { get => m_isTriggered; }
        public float Progress { get => m_scannedAmount; 
            set {
                m_scannedAmount = Mathf.Clamp01(value);
            } 
        }

        protected virtual void Awake() {
            m_gizmo = GetComponent<ScannerGizmo>();
            m_scanTrigger = GetComponent<Collider2D>();
            m_scanTrigger.isTrigger = true;
        }

        protected virtual void Update() {
            if(m_action == null)
                return;

            m_action.Calculate(this, m_scanIncreaseAmount);
            m_scanSlider.value = m_scannedAmount;

            if(Progress >= 1f) {
                onScanCompleted?.Invoke();
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) {
            if(m_isTriggered) 
                return;

            if(collision.CompareTag("Player") == false)
                return;

            m_isTriggered = true;
            onTriggerStartAction();
        }

        protected virtual void OnTriggerExit2D(Collider2D collision) {
            if(m_isTriggered == false)
                return;

            if(collision.CompareTag("Player") == false)
                return;

            m_isTriggered = false;
            onTriggerEndAction();
        }

        private void OnEnable() {
            m_gizmo.ActivateScan();
            m_scanCanvas.gameObject.SetActive(true);
        }

        private void OnDisable() {
            m_gizmo.DeactivateScan();
            m_scanCanvas.gameObject.SetActive(false);
        }

        protected virtual void onTriggerStartAction() {

        }

        protected virtual void onTriggerEndAction() {

        }
    }
}