using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public class SearchableBase : MonoBehaviour {
        [SerializeField] protected string m_IDFormat = string.Empty;
        [SerializeField] protected string m_ID = string.Empty;
        [SerializeField] protected string m_Location = string.Empty;
        [SerializeField, Multiline(10)] protected string m_Description = string.Empty;
        [SerializeField] protected Vector3 m_position = Vector3.zero;
        [SerializeField] protected ObjectType m_ObjectType;
        [SerializeField] protected StatusType m_Status;

        #region Properties
        public virtual string ID { get => m_ID; }
        public virtual string Location { get => m_Location; }
        public virtual string Description { get => m_Description; }
        public virtual Vector3 Position { get => m_position; }
        public virtual ObjectType ObjectType { get => m_ObjectType; }
        public virtual StatusType Status { get => m_Status; }

        #endregion

        protected ItemData m_objectData;

        private void Start() {
            //ItemManager.Instance?.AddItemData(this);
        }

        /// <summary>
        /// ID에 포함되는지 체크하는 함수.
        /// </summary>
        /// <param name="objectCode"></param>
        /// <returns></returns>
        public bool Contains(string objectCode) {
            if(objectCode == null)
                return false;

            if(m_ID.Contains(objectCode.ToUpper()) || m_Location.Contains(objectCode.ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// 오브젝트의 ID와 일치하는지 체크하는 함수
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public bool CheckValidID(string objectID) {
            if(objectID == null)
                return false;

            return m_ID.Equals(objectID.ToUpper());
        }

        /// <summary>
        /// 오브젝트의 Location과 일치하는지 체크하는 함수
        /// </summary>
        /// <param name="objectLocation"></param>
        /// <returns></returns>
        public bool CheckValidLocation(string objectLocation) {
            if(objectLocation == null)
                return false;

            return m_Location.Equals(objectLocation.ToUpper());
        }

        /// <summary>
        /// 오브젝트의 <see cref="ItemData"/>반환하는 함수.
        /// </summary>
        /// <returns></returns>
        public virtual ItemData GetItemData() {
            return m_objectData;
        }

        private void OnDisable() {
            //ItemManager.Instance?.RemoveItemData(this);
        }
    }
}

