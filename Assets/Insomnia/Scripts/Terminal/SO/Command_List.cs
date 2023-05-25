using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/Search/List", fileName = "Command_List")]
    public class Command_List : TerminalCommand {
        private static string m_listStartFormat = "ID\t\tObjectType\t\tSTATUS";
        private static string m_listFormat = "{0}\t\t{1}\t\t\t{2}";
        private static string m_listEmptyFormat = "There is no item ID: {0}";
        private static string m_listEmptyFormatWithArea = "There is no item ID: {0} in Area: {1}";

        public override KeyValuePair<float, List<string>> RunCommand(TerminalUI console, string command) {
            m_commandResult.Clear();
            m_commandResult.Add(m_listStartFormat);
            string[] keys = command.Split(' ');

            string itemID = string.Empty;
            string areaID = string.Empty;

            for(int i = 1; i < keys.Length; i++) {
                if(keys[i].Contains("AREA_"))
                    areaID = keys[i];
                else
                    itemID = keys[i];
            }

            ItemData[] datas = default(ItemData[]);

            switch(keys.Length - 1) {
                case 0: datas = ItemManager.Instance.GetItemDatas(null); break;
                case 1: datas = ItemManager.Instance.GetItemDatas(itemID);break;
                case 2: datas = ItemManager.Instance.GetItemDatas(itemID, areaID); break;
            }

            if(datas.Length <= 0) { //데이터가 없거나 잘못 입력된 경우

            }
            else {//제대로 데이터를 전달받았을 경우
                for(int i = 0; i < datas.Length; i++) {
                    m_commandResult.Add(string.Format(m_listFormat, datas[i].ID, datas[i].ObjectType.ToString(), datas[i].Status.ToString()));
                }
            }

            return new KeyValuePair<float, List<string>>(m_loadingTime, m_commandResult);
        }
    }
}