using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia{
	public class UplinkableBase : SearchableBase {
        protected override void Reset() {
            m_IDFormat = "PUNCHINGCARD";
            m_Location = "ZONE_";
            m_ObjectType = ObjectType.Security;
            m_Status = StatusType.Normal;
        }

        protected override void Start() {
            base.Start();

			Uplink uplink = Uplink.Instance;
			if(uplink == null)
				return;

			uplink.AddUplinkItem(this);
		}
	}
}
