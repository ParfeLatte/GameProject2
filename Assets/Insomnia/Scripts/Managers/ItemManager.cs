using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Linq;
using System;
using static Insomnia.Defines;

namespace Insomnia {
    public class ItemManager : Singleton<ItemManager> {
        //TODO: ItemData�� �Ź� �����ϴ°� �ƴ� �߰��� �� ���� �̸� ���������� ���� �ʿ�.
        [SerializeField] private List<ItemBase> m_itemDatas = new List<ItemBase>();

        public void AddItemData(ItemBase item) {
            if(m_itemDatas.Contains(item) == true)
                return;

            m_itemDatas.Add(item); 
        }

        public void RemoveItemData(ItemBase item) {
            if(m_itemDatas.Contains(item) == false)
                return;

            m_itemDatas.Remove(item);
        }

        public ItemData[] GetItemDatas(string key) {
            ItemBase[] items;

            if(key == null)
                items = m_itemDatas.ToArray();
            else
                items = m_itemDatas.Where(x => x.Contains(key)).ToArray();

            ItemData[] ret = new ItemData[items.Length];
            for(int i = 0; i < items.Length; i++) {
                ret[i] = items[i].GetItemData();
            }

            return ret;
        }
    }
}