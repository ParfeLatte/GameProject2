using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName ="Command/SaveLoad/Save", fileName ="Command_Save")]
    public class Command_Save : CommandSO {
        private static string m_saveSaving = "Saveing Data...";
        private static string m_saveSaveCompleted = "Save Completed!";

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal_Searchable terminal, string command) {
            m_commandResult.Clear();
            m_commandResult.Add(m_saveSaving);

            yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);
            m_commandResult.Clear();
            GameManager.Instance.Save();
            m_commandResult.Add(m_saveSaveCompleted);
            yield return new KeyValuePair<float, List<string>>(3f, m_commandResult);
        }
    }
}