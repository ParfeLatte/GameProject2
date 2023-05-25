using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public class Item : SearchableBase {
        public void Awake() {
            m_ID = m_IDFormat + Random.Range(0, 1000).ToString("000");
            m_position = transform.position;

            m_objectData = new ItemData() {
                ID = m_ID,
                Location = m_Location,
                Description = m_Description,
                Position = m_position,
                ObjectType = m_ObjectType,
                Status = m_Status
            };
        }

        private void Start() {
            ItemManager.Instance.AddItemData(this);
        }
    }

}
