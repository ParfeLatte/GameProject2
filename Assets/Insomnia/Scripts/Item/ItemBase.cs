using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public class ItemBase : MonoBehaviour {
        [SerializeField] protected string m_ID = string.Empty;
        [SerializeField] protected string m_Location = string.Empty;
        [SerializeField] protected string m_Description = string.Empty;
        [SerializeField] protected Vector3 m_position = Vector3.zero;
        [SerializeField] protected ObjectType m_ObjectType;
        [SerializeField] protected StatusType m_Status;

        ItemData m_ItemData;

        public virtual void Awake() {
            //TODO: 위치랑 로케이션 찾아서 갖고있기
            m_ItemData = new ItemData() {
                ID = m_ID,
                Location = m_Location,
                Description = m_Description,
                Pos = m_position,
                ObjectType = m_ObjectType,
                Status = m_Status
            };
        }

        private void Start() {
            ItemManager.Instance?.AddItemData(this);
        }

        public bool Contains(string objectCode) {
            if(objectCode == null)
                return false;

            if(m_ID.Contains(objectCode.ToUpper()) || m_Location.Contains(objectCode.ToUpper()))
                return true;

            return false;
        }
        public bool CheckValidID(string objectID) {
            if(objectID == null)
                return false;

            return m_ID.Equals(objectID.ToUpper());
        }
        public bool CheckValidLocation(string objectLocation) {
            if(objectLocation == null)
                return false;

            return m_Location.Equals(objectLocation.ToUpper());
        }
        public virtual ItemData GetItemData() {
            return m_ItemData;
        }

        private void OnDisable() {
            ItemManager.Instance?.RemoveItemData(this);
        }

        private void OnDestroy() {
            OnDisable();
        }
    }
}

