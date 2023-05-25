using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/Search/Query", fileName = "Command_Query")]
    public class Command_Query : TerminalCommand {
        private static string m_queryStartFormat =
            "--------------------------------------------------------------------------------\n{0}\n--------------------------------------------------------------------------------";
        private static string m_queryFormat = "ID: {0}\nITEM STATUS: {1}\nLOCATION: {2}\nPING STATUS: {3}";
        private static string m_pingFailFormat = "Ping out of range. Get to {0} to get close enough to find the item.";
        public override KeyValuePair<float, List<string>> RunCommand(TerminalUI console, string command) {
            m_commandResult.Clear();
            string[] keys = command.Split(' ');

            ItemData[] datas;

            if(keys.Length > 1) {
                datas = ItemManager.Instance.GetItemDatas(keys[1], isForQuery: true);
            }
            else
                datas = default(ItemData[]);

            if(datas.Length == 1) {
                m_commandResult.Add(string.Format(m_queryStartFormat, datas[0].Description));
                string pingResult = string.Format(m_pingFailFormat, datas[0].ID);
                m_commandResult.Add(string.Format(m_queryFormat, datas[0].ID, datas[0].Status, datas[0].Location, pingResult));
            }

            return new KeyValuePair<float, List<string>>(m_loadingTime, m_commandResult);
        }
    }
}