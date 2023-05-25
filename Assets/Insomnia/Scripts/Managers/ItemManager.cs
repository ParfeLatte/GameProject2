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
        [SerializeField] private List<Item> m_itemDatas = new List<Item>();

        public void AddItemData(Item item) {
            if(m_itemDatas.Contains(item) == true)
                return;

            m_itemDatas.Add(item); 
        }

        public void RemoveItemData(Item item) {
            if(m_itemDatas.Contains(item) == false)
                return;

            m_itemDatas.Remove(item);
        }

        public ItemData[] GetItemDatas(string itemID, bool isForQuery = false) {
            SearchableBase[] items;

            if(isForQuery) 
                items = m_itemDatas.Where(x => x.CheckValidID(itemID)).ToArray();
            else {
                if(itemID == null)
                    items = default(SearchableBase[]);
                if(itemID == "all")
                    items = m_itemDatas.ToArray();
                else
                    items = m_itemDatas.Where(x => x.Contains(itemID)).ToArray();
            }

            ItemData[] ret = new ItemData[items.Length];
            for(int i = 0; i < items.Length; i++) {
                ret[i] = items[i].GetItemData();
            }

            return ret;
        }

        public ItemData[] GetItemDatas(string itemID, string locationID, bool isForQuery = false) {
            //로케이션ID로 아이템 찾는 기능 만들기
            SearchableBase[] items = default(SearchableBase[]);
            ItemData[] ret = default(ItemData[]);
            if(isForQuery) {
                items = m_itemDatas.Where(x => x.CheckValidID(itemID) && x.CheckValidLocation(locationID)).ToArray();
            }
            else {
                if(itemID == null)
                    items = default(SearchableBase[]);
                if(itemID == "all") {

                }

            }

            return ret;
        }
    }
} 