using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia{
	public class Uplink : MortalSingleton<Uplink> {
		[Header("Uplink: External References")]
		[SerializeField] private GameObject m_uplinkScanner = null;

		[Header("Uplink: Status")]
		[SerializeField] private string m_uplinkItemID = "";

		public void AddUplinkItem(UplinkableBase item) {
			if(item == null)
				return;

			m_uplinkItemID = item.Data.ID;
        }

		public bool CheckUplinkData(string itemID) {
			if(m_uplinkItemID == string.Empty)
				return false;

			if(m_uplinkScanner == null)
				return false;

			if(m_uplinkItemID.Contains(itemID) == false)
				return false;

			m_uplinkScanner.SetActive(true);
			return true;
		}
	}
}
