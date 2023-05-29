using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName ="Command/TerminalControl/Command", fileName ="Command_Command")]
    public class Command_Command : TerminalCommand {

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal terminal, string command) {
            m_commandResult.Clear();

            for(int i = 0; i < terminal.UI.Commands.Count; i++) {
                m_commandResult.Add(terminal.UI.Commands[i].GetDescription());
            }

            yield return new KeyValuePair<float, List<string>>(m_loadingTime, m_commandResult);
        }
    }
}