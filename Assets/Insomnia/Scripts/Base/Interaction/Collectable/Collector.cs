using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Insomnia.Defines;
using static UnityEditor.Progress;

namespace Insomnia{
	public class Collector : MonoBehaviour {
		[Header("Collector: External References")]
		[SerializeField]
		private List<CollectableBase> m_collected = new List<CollectableBase>();

		public CollectableBase GetItem(CollectableType m_itemType) {
			//CollectableBase item = m_collected.SingleOrDefault(x => x.Data.Type == m_itemType);

			//if(item != null)
			//	m_collected.Remove(item);

			return m_collected.SingleOrDefault(x => x.ColData.Type == m_itemType);
        }

		public CollectableBase GetItemContaining(string itemName) {
			return m_collected.FirstOrDefault(x => x.ColData.ObjData.ID.Contains(itemName));
		}

		public void AddItem(CollectableBase item) {
			if(m_collected.Contains(item)) 
				return;

			m_collected.Add(item);
		}
    }
}
