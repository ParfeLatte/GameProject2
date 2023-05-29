using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/TerminalControl/Exit", fileName = "Command_Exit")]
    public class Command_Exit : TerminalCommand {

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal terminal, string command) {
            terminal.UI.CloseTerminal();

            yield return default;
        }
    }
}