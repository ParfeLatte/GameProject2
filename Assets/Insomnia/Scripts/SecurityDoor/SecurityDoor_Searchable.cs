using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia{
    public class SecurityDoor_Searchable : SearchableBase {
        protected override void Reset() {
            m_IDFormat = "DOOR";
            m_Location = "ZONE_";
            m_ObjectType = ObjectType.Passage;
            m_Status = StatusType.Normal;
        }
    }
}
