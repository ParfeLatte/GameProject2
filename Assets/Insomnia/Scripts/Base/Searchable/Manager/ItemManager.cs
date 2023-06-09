using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Linq;
using System;
using static Insomnia.Defines;

namespace Insomnia {
    public class ItemManager : MortalSingleton<ItemManager> {
        [SerializeField] private List<SearchableBase> m_itemDatas = new List<SearchableBase>();

        public void AddSearchable(SearchableBase item) {
            if(m_itemDatas.Contains(item) == true)
                return;

            m_itemDatas.Add(item); 
        }

        public void RemoveItemData(SearchableBase item) {
            if(m_itemDatas.Contains(item) == false)
                return;

            m_itemDatas.Remove(item);
        }

        public void RemoveAllItemDatas(Player player) {
            if(player.isDead == false)
                return;

            m_itemDatas.Clear();
        }

        public KeyValuePair<CommandError, ObjectData[]> GetItemDatasForList(Terminal_Searchable terminal, string itemID, string locationID) {
            SearchableBase[] items = default(SearchableBase[]);
            ObjectData[] ret = default(ObjectData[]);
            CommandError errorType = CommandError.Success;

            if(itemID == null)
                items = default(SearchableBase[]);
            if(itemID == "ALL") {
                if(locationID != string.Empty)
                    errorType = CommandError.SyntaxError_ExceptionalParam;
                else
                    items = m_itemDatas.ToArray();
            }
            if(itemID != "ALL") {
                if(locationID == string.Empty)
                    items = m_itemDatas
                    .Where(x => x.Contains(itemID) || x.CheckValidID(itemID)).ToArray();
                else
                    items = m_itemDatas
                    .Where(x => (x.CheckValidLocation(locationID) && (x.Contains(itemID) || x.CheckValidID(itemID)))).ToArray();
            }

            if(items != null) {
                ret = new ObjectData[items.Length];
                for(int i = 0; i < items.Length; i++) {
                    ret[i] = items[i].Data;
                }
            }

            return new KeyValuePair<CommandError, ObjectData[]>(errorType, ret);
        }

        public KeyValuePair<CommandError, ObjectData[]> GetItemDataForQuery(Terminal_Searchable terminal, string itemID, string lcoationID) {
            SearchableBase[] items = default(SearchableBase[]);
            ObjectData[] ret = default(ObjectData[]);
            CommandError errorType = CommandError.Success;

            items = new SearchableBase[1]{ m_itemDatas.SingleOrDefault(x => x.CheckValidID(itemID)) };
            if(items.Length <= 0)
                errorType = CommandError.InvalidID;

            ret = new ObjectData[items.Length];
            for(int i = 0; i < items.Length; i++) {
                ret[i] = items[i].Data;
            }

            return new KeyValuePair<CommandError, ObjectData[]>(errorType, ret);
        }

        public string GetItemID(string leakedKey) {
            if(leakedKey == null || leakedKey == string.Empty)
                return string.Empty;
            SearchableBase item = m_itemDatas.FirstOrDefault(x => x.Contains(leakedKey));

            if(item == null)
                return string.Empty;

            return item.IDFormat;
        }

        public bool CheckItemExists(string itemID) {
            return m_itemDatas.Any(x => x.CheckValidID(itemID));
        }
    }
} 