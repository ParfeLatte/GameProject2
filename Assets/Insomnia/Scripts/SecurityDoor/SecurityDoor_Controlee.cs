using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Insomnia.Door_Speaker;

namespace Insomnia {
    public class SecurityDoor_Controlee : ControleeBase {
        [Header("SecurityDoor: Components")]
        [SerializeField] private Animator m_animator = null;
        [SerializeField] private Door_Speaker m_speaker = null;
        [SerializeField] private BoxCollider2D m_collider = null;

        [Header("SecurityDoor: Status")]
        [SerializeField] private bool m_Activation = false;
        private bool m_targetActivation = false;

        [Header("SecurityDoor: Settings")]
        [SerializeField, Tooltip("Parameter Name of \"Use\"")] 
        private string m_hashUseString = "";
        private int m_hashUse = -1;

        #region Properties
        public bool IsOpened { get => m_Activation; set => m_Activation = value; }

        #endregion

        #region Unity Event Functions
        private void Awake() {
            m_animator = GetComponentInChildren<Animator>();

            if(m_animator == null)
                return;

            m_hashUse = Animator.StringToHash(m_hashUseString);
        }

        private void Update() {
            if(m_isReacting == false)
                return;

            Activation();
        }

        #endregion

        #region ReactorBase Functions
        public override void StandbyInteract() {

        }

        public override void ReleaseInteract() {

        }

        public override bool OnInteractStart(object request) {
            m_targetActivation = !m_Activation;
            m_isReacting = true;
            m_animator.SetTrigger(m_hashUse);
            return true;
        }

        public override bool OnInteractEnd() {
            return true;
        }
        #endregion

        private void Activation() {
            if(m_targetActivation == m_Activation) {
                m_isReacting = false;
                return;
            }

            if(SetDoorStatus(m_targetActivation) == false)
                return;
        }

        private bool SetDoorStatus(bool activation) {
            m_speaker.Play(activation ? (int)DoorSounds.DoorOpen : (int)DoorSounds.DoorClose);

            AnimatorStateInfo animStateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if(animStateinfo.IsName("SecurityDoor_" + ( activation ? "Open" : "Close" )) == false)
                return false;

            Vector2 colliderSize = m_collider.size;
            colliderSize = new Vector2(colliderSize.x, Mathf.Clamp01(colliderSize.y + ( activation ? -1 : 1 ) * ( animStateinfo.normalizedTime )));

            bool enabled = colliderSize.y > 0.1f;
            m_collider.enabled = enabled;

            if(enabled)
                m_collider.size = colliderSize;

            //문 여닫기 종료
            if(animStateinfo.normalizedTime >= 1f) {
                m_Activation = activation;
                return true;
            }

            return false;
        }
    }
}