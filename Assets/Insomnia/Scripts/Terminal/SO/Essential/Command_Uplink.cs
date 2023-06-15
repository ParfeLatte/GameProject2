using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia{
    [CreateAssetMenu(menuName = "Command/Special/Uplink", fileName = "Command_Uplink")]
    public class Command_Uplink : CommandSO {
        private static string m_uplinkStartFormat = "Wait for Uplink Scan...";
        private static string m_uplinkAttributeExceptionErrorFormat = "Uplink Error: Invalid ID. Check Attributes";
        private static string m_uplinkItemIsNotValid = "Uplink Error: Uplink Failed. Wait For the Signal";

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal_Searchable terminal, string command) {
            m_commandResult.Clear();

            Collector collector = terminal.Interactable.User.gameObject.GetComponent<Collector>();
            if(collector == null)
                yield break;

            string[] keys = command.Split(' ');
            if(keys.Length != 2) {
                m_commandResult.Add(m_uplinkAttributeExceptionErrorFormat);
                yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);
                yield break;
            }

            string itemID = keys[1];

            CollectableBase item = collector.GetItemContaining(itemID);
            if(item == null) {
                m_commandResult.Add(m_uplinkAttributeExceptionErrorFormat);
                yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);
                yield break;
            }

            bool success = Uplink.Instance.CheckUplinkData(itemID);
            if(success == false) {
                m_commandResult.Add(m_uplinkItemIsNotValid);
                yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);
                yield break;
            }
            
            m_commandResult.Add(m_uplinkStartFormat);
            yield return new KeyValuePair<float, List<string>>(LoadingTime, m_commandResult);
            yield break;
        }
    }
}
