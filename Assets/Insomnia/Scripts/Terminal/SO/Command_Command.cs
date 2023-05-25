using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName ="Command/TerminalControl/Command", fileName ="Command_Command")]
    public class Command_Command : TerminalCommand {

        public override KeyValuePair<float, List<string>> RunCommand(TerminalUI console, string command) {
            m_commandResult.Clear();

            for(int i = 0; i < console.Commands.Count; i++) {
                m_commandResult.Add(console.Commands[i].GetDescription());
            }

            return new KeyValuePair<float, List<string>>(m_loadingTime, m_commandResult);
        }
    }
}