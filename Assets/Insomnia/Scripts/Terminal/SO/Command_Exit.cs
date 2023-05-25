using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/TerminalControl/Exit", fileName = "Command_Exit")]
    public class Command_Exit : TerminalCommand {

        public override KeyValuePair<float, List<string>> RunCommand(TerminalUI console, string command) {
            console.CloseTerminal();
            return new KeyValuePair<float, List<string>>();
        }
    }
}