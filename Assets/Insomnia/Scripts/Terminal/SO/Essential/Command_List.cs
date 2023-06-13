using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/Search/List", fileName = "Command_List")]
    public class Command_List : CommandSO {
        #region Formats
        private static string m_listStartFormat = "ID\t\tObjectType\t\tSTATUS";
        private static string m_listFormat = "{0, -12}\t{1, -23}\t{2}";
        private static string m_listWaitFormat = "Loading For Data: {0} Location: {1}";
        private static string m_listSynParamError = "<color=red>Command Error: Not authorized params detected</color>";
        private static string m_listInvalidIDError = "<color=red>Command Error: Invalid ID</color>";
        #endregion

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal terminal, string command) {
            m_commandResult.Clear();
            string[] keys = command.Split(' ');
            if(keys.Length > 3 || keys.Length <= 1) {
                m_commandResult.Add(m_listSynParamError);
                yield return new KeyValuePair<float, List<string>>(0, m_commandResult);
                yield break;
            }

            string itemID = string.Empty;
            string locationID = string.Empty;

            for(int i = 1; i < keys.Length; i++) {
                if(keys[i].Contains("ZONE_"))
                    locationID = keys[i];
                else
                    itemID = keys[i];
            }

            m_commandResult.Add(string.Format(m_listWaitFormat, itemID != string.Empty ? itemID : "NULL", locationID != string.Empty ? locationID : "NULL"));

            yield return new KeyValuePair<float, List<string>>(0, m_commandResult);
            m_commandResult.Clear();
            m_commandResult.Add(m_listStartFormat);

            KeyValuePair<CommandError, ObjectData[]> result = ItemManager.Instance.GetItemDatasForList(terminal, itemID, locationID);

            switch(result.Key) {
                case CommandError.Success: {
                    for(int i = 0; i < result.Value.Length; i++) {
                        m_commandResult.Add(string.Format(m_listFormat, result.Value[i].ID, result.Value[i].ObjectType.ToString(), result.Value[i].Status.ToString()));
                    }
                }
                break;
                case CommandError.InvalidID: {
                    m_commandResult.Add(m_listInvalidIDError);
                }break;
                case CommandError.SyntaxError_ExceptionalParam: {
                    m_commandResult.Add(m_listSynParamError);
                }break;
                default:break;
            }

            yield return new KeyValuePair<float, List<string>>(m_loadingTime, m_commandResult);
        }
    }
}