using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/Search/Query", fileName = "Command_Query")]
    public class Command_Query : CommandSO {
        #region Formats
        private static string m_queryStartFormat =
            "--------------------------------------------------------------------------------\n{0}\n--------------------------------------------------------------------------------";
        private static string m_queryFormat = "ID: {0}\nITEM STATUS: {1}\nLOCATION: {2}\nPING STATUS: {3}";
        private static string m_pingFailFormat = "Ping out of range. Get close enough to {0} to find the item.";
        private static string m_queryEmptyFormat = "There is no item ID: {0}";
        private static string m_querySynParamError = "<color=red>Command Error: Not authorized params detected</color>";
        private static string m_queryEmptyFormatWithArea = "There is no item ID: {0} in Area: {1}";
        private static string m_queryInvalidIDError = "<color=red>Command Error: Invalid ID</color>";
        #endregion
        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal_Searchable terminal, string command) {
            m_commandResult.Clear();
            string[] keys = command.Split(' ');

            if(keys.Length != 2) {
                m_commandResult.Add(m_querySynParamError);
                yield return new KeyValuePair<float, List<string>>(0, m_commandResult);
                yield break;
            }

            if(ItemManager.Instance.CheckItemExists(keys[1]) == false) {
                m_commandResult.Add(m_queryInvalidIDError);
                yield return new KeyValuePair<float, List<string>>(0, m_commandResult);
                yield break;
            }

            KeyValuePair<CommandError, ObjectData[]> datas = ItemManager.Instance.GetItemDataForQuery(terminal, keys[1], terminal.Location);

            switch(datas.Key) {
                case CommandError.Success: {
                    m_commandResult.Add(string.Format(m_queryStartFormat, datas.Value[0].Description));
                    string pingResult = string.Format(m_pingFailFormat, datas.Value[0].ID);
                    m_commandResult.Add(string.Format(m_queryFormat, datas.Value[0].ID, datas.Value[0].Status, datas.Value[0].Location, pingResult));
                }
                break;
            }

            yield return new KeyValuePair<float, List<string>>(m_loadingTime, m_commandResult);
        }
    }
}