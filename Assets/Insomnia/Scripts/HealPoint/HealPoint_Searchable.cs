using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia{
	public class HealPoint_Searchable : SearchableBase {
        protected override void Reset() {
            m_IDFormat = "HEALPOINT";
            m_Location = "ZONE_";
            m_ObjectType = ObjectType.Resources;
            m_Status = StatusType.Normal;
        }
    }
}
