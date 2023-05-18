using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/Local/List", fileName = "Command_List")]
    public class Command_List : ConsoleCommand {
        private static string m_listStartFormat = "ID\t\tObjectType\t\tSTATUS\n";
        private static string m_listFormat = "{0}\t\t{1}\t\t{2}\n";
        public override bool CheckCommand(string splitted) {
            return m_command.Equals(splitted.ToUpper());
        }

        public override KeyValuePair<float, List<string>> RunCommand(ConsoleUI console, string command) {
            m_commandResult.Clear();
            m_commandResult.Add(m_listStartFormat);
            string[] keys = command.Split(' ');

            ItemData[] datas;

            if(keys.Length > 1) {
                datas = ItemManager.Instance.GetItemDatas(keys[1]);
            }
            else
                datas = ItemManager.Instance.GetItemDatas(null);
            for(int i = 0; i < datas.Length; i++) {
                m_commandResult.Add(string.Format(m_listFormat, datas[i].ID, datas[i].ObjectType.ToString(), datas[i].Status.ToString()));
            }

            return new KeyValuePair<float, List<string>>(m_loadingTime, m_commandResult);
        }
    }
}