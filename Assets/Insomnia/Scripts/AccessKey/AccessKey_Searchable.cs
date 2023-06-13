using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia{
    public class AccessKey_Searchable : SearchableBase {
        protected override void Reset() {
            m_IDFormat = "KEY_";
            m_Location = "ZONE_";
            m_ObjectType = ObjectType.Security;
            m_Status = StatusType.Normal;
        }
    }
}
