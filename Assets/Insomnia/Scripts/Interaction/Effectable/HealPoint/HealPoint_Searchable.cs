using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia{
	public class HealPoint_Searchable : SearchableBase {
        private void OnValidate() {
            m_IDFormat = "HEALPOINT";
            m_Location = "ZONE_";
            m_ObjectType = ObjectType.Resources;
            m_Status = StatusType.Normal;
        }
    }
}
