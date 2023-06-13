using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/Essential/Help", fileName = "Command_Help")]
    public class Command_Help : CommandSO {
        private static string m_helpDefaultFormat = "Use the keyboard to write commands.\nUse [Enter/Return] to execute commands.\nUse [Backspace] to erase a character.\nUse [TAB] to autocomplete your command.\n\nType \"COMMANDS\" to get a list of all available commands.\r\n";

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal terminal, string command) {
            m_commandResult.Clear();
            m_commandResult.Add(m_helpDefaultFormat);
            yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);
            yield break;
        }
    }
}

