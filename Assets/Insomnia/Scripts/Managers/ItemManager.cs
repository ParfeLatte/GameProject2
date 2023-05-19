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
        //TODO: ItemData를 매번 생성하는게 아닌 추가될 때 같이 미리 만들어놓도록 수정 필요.
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

        public ItemData[] GetItemDatas(string key, bool isForQuery = false) {
            ItemBase[] items;

            if(isForQuery) 
                items = m_itemDatas.Where(x => x.CheckValidID(key)).ToArray();
            else {
                if(key == null)
                    items = default(ItemBase[]);
                if(key == "all")
                    items = m_itemDatas.ToArray();
                else
                    items = m_itemDatas.Where(x => x.Contains(key)).ToArray();
            }

            ItemData[] ret = new ItemData[items.Length];
            for(int i = 0; i < items.Length; i++) {
                ret[i] = items[i].GetItemData();
            }

            return ret;
        }

        private void OnApplicationQuit() {
            m_itemDatas.Clear();
        }
    }
}