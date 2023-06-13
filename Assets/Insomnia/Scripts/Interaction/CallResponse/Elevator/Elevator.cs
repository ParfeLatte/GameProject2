using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;
using static Insomnia.ElevatorSpeaker;

namespace Insomnia {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Elevator : ControleeBase {
        [Header("Elevator: Components")]
        [SerializeField] private SpriteRenderer m_renderer = null;
        [SerializeField] private Animator m_animator = null;
        [SerializeField] private BoxCollider2D m_onRideTrigger = null;

        [Header("Elevator: Internal References")]
        [SerializeField] private ElevatorSpeaker m_speaker = null;
        [SerializeField] private ControllerBase[] m_controllers = null;
        [SerializeField] private BoxCollider2D[] m_doorColliders = null;

        [Header("Elevator: External References")]
        [SerializeField] private Transform[] m_targetFloors = null;

        [Header("Elevator: Status")]
        [SerializeField] private int m_targetFloorIndex = -1;
        [SerializeField] private int m_currentFloorIndex = -1;
        [SerializeField] private bool m_isOpened = false;
        [SerializeField] private bool m_doorReaching = false;

        [Header("Elevator: Settings")]
        [SerializeField, Range(1f, 30f)]private float m_elevatorMoveSpeed = 10f;

        #region Properties
        /// <summary>
        /// Function for trigger m_isOpened
        /// </summary>
        //private bool IsOpened {
        //    get {
        //        if(m_isOpened == false)
        //            return false;

        //        m_isOpened = false;
        //        return true;
        //    }
        //}

        public int CurrentFloor { get => m_currentFloorIndex; }
        public bool IsReacting { get => m_isReacting; }

        #endregion

        #region Unity Event Functions

        private void Awake() {
            m_speaker = GetComponentInChildren<ElevatorSpeaker>();
        }

        private void Update() {
            if(m_isReacting == false)
                return;

            Elevate();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.CompareTag("Player") == false)
                return;

            collision.transform.SetParent(transform);
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if(collision.CompareTag("Player") == false)
                return;

            collision.transform.SetParent(null);
        }

        #endregion

        #region ReactorBase Functions
        public sealed override void StandbyInteract() { }

        public sealed override void ReleaseInteract() { }

        public sealed override bool OnInteractStart(object request) {
            int req_int = (int)request;

            #region Validation Check for OutofRange Exception Error
            if(req_int < 0)
                return false;

            if(m_targetFloors.Length == 0)
                return false;

            if(m_targetFloors.Length - 1 < req_int)
                return false;

            if(m_targetFloors[req_int] == null)
                return false;

            #endregion

            #region Validation Check for Logic
            if(m_isReacting)
                return false;

            #endregion

            m_targetFloorIndex = req_int;
            m_isReacting = true;
            return true;
        }

        public sealed override bool OnInteractEnd() { return true; }

        #endregion

        private void Elevate() {
            #region Validation Check for OutofRange Exception Error
            if(m_targetFloorIndex < 0)
                return;

            if(m_targetFloors.Length <= m_targetFloorIndex)
                return;

            #endregion

            //Validation Check for Elevator already Reached
            if(m_currentFloorIndex == m_targetFloorIndex) {
                bool opened = SetDoorStatus(true);

                if(opened) 
                    m_isReacting = false;

                return;
            }

            //Validation Check for Elevator Door Closed
            if(SetDoorStatus(false) == false)
                return;

            Vector3 distance = m_targetFloors[m_targetFloorIndex].position - transform.position;

            //Validation Check for Elevator Arrived
            if(distance.magnitude >= 0.1f) {
                Vector3 direction = distance.normalized;
                transform.Translate(direction * m_elevatorMoveSpeed * Time.deltaTime);
                m_speaker.Play((int)ElevatorSounds.Move, true);
                return;
            }
            else { 
                transform.position = m_targetFloors[m_targetFloorIndex].position;
                m_currentFloorIndex = m_targetFloorIndex;
                m_speaker.Stop();
            }
        }


        /// <summary>
        /// 엘레베이터의 문을 열고 닫을 때 호출하는 함수.
        /// </summary>
        /// <param name="status">true to Open, false to Close</param>
        /// <returns>return true if Changing Door State is Finished. else false.</returns>
        private bool SetDoorStatus(bool status) {
            if(m_isOpened == status)
                return true;

            if(m_doorReaching == false) {
                m_animator.SetBool("DoorStatus", status);
                m_speaker.Play((int)(status ? ElevatorSounds.Open : ElevatorSounds.Close), false);
                m_doorReaching = true;
                return false;
            }

            AnimatorStateInfo animStateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if(animStateinfo.IsName("Elevator_" + (status ? "Open" : "Close")) == false)
                return false;

            //Calculating Collider's size.
            Vector2 size = Vector2.zero;
            if(m_doorColliders.Length> 0) {
                size = m_doorColliders[0].size;
                size = new Vector2(size.x, Mathf.Clamp01(size.y + ( status ? -1 : 1 ) * ( animStateinfo.normalizedTime )));
            }

            //Adjusting calculated size to doorColliders.
            for(int i = 0; i < m_doorColliders.Length; i++) {
                bool enabled = size.y > 0.1f;
                m_doorColliders[i].enabled = enabled;
                if(enabled)
                    m_doorColliders[i].size = size;
            }
                 
            //문 여닫기 종료
            if(animStateinfo.normalizedTime >= 1f) {
                m_doorReaching = false;
                m_isOpened = status;

                //문을 닫을 때 이동하는 소리 재생 시작
                if(status == false) {
                    m_speaker.Stop();
                    m_speaker.Play((int)ElevatorSounds.Move, true);
                }
                    
                return true;
            }

            return false;
        }
    }
}