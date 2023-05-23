using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public class ItemBase : MonoBehaviour {
        protected List<string> m_commandResult = new List<string>();
        [SerializeField] protected string m_ID = "";
        [SerializeField] protected string m_Location = "";
        [SerializeField] protected Vector3 m_position = Vector3.zero;
        [SerializeField] protected ObjectType m_ObjectType;
        [SerializeField] protected StatusType m_Status;

        public virtual void Awake() {
            //TODO: 위치랑 로케이션 찾아서 갖고있기
            
        }

        private void Start() {
            ItemManager.Instance?.AddItemData(this);
        }

        public bool Contains(string objectCode) {
            if(m_ID.Contains(objectCode.ToUpper()))
                return true;

            return false;
        }

        public virtual ItemData GetItemData() {
            ItemData ret = new ItemData(){
                ID = m_ID,
                Location = m_Location,
                ObjectType = m_ObjectType,
                Status = m_Status
            };

            return ret;
        }
    }
}

