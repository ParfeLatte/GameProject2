using System.Collections;
using System.Collections.Generic;
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
        protected ObjectData m_objectData;

        #region Properties
        public string IDFormat { get => m_IDFormat; }
        public string ID { get => m_ID; }
        public string Location { get => m_Location; }
        public string Description { get => m_Description; }
        public Vector3 Position { get => m_position; }
        public ObjectType ObjectType { get => m_ObjectType; }
        public StatusType Status { get => m_Status; }
        public ObjectData Data { get => m_objectData; }
        #endregion

        /// <summary>
        /// ID�� ���ԵǴ��� üũ�ϴ� �Լ�.
        /// </summary>
        /// <param name="objectCode"></param>
        /// <returns></returns>
        public bool Contains(string objectCode) {
            if(objectCode == null)
                return false;

            if(m_ID.StartsWith(objectCode) == false)
                return false;

            if(m_ID.Contains(objectCode))
                return true;

            return false;
        }

        /// <summary>
        /// ������Ʈ�� ID�� ��ġ�ϴ��� üũ�ϴ� �Լ�
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public bool CheckValidID(string objectID) {
            if(objectID == null)
                return false;

            return m_ID.Equals(objectID);
        }

        /// <summary>
        /// ������Ʈ�� Location�� ��ġ�ϴ��� üũ�ϴ� �Լ�
        /// </summary>
        /// <param name="objectLocation"></param>
        /// <returns></returns>
        public bool CheckValidLocation(string objectLocation) {
            if(objectLocation == null)
                return false;

            return m_Location.Equals(objectLocation);
        }

        protected virtual void Awake() {
            m_ID = m_IDFormat + '_' + Random.Range(0, 1000).ToString("000");
            m_position = transform.position;
        }

        protected virtual void Start() {
            while(true) {
                m_ID = m_IDFormat + '_' + Random.Range(0, 1000).ToString("000");
                if(ItemManager.Instance.CheckItemExists(m_ID) == false)
                    break;
            }

            m_objectData = new ObjectData() {
                ID = m_ID,
                Location = m_Location,
                Description = m_Description,
                Position = m_position,
                ObjectType = m_ObjectType,
                Status = m_Status
            };

            ItemManager.Instance.AddSearchable(this);
        }

        private void OnDisable() {
            //ItemManager.Instance?.RemoveItemData(this);
        }
    }
}

