using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName ="Command/TerminalControl/Clear", fileName ="Command_Clear")]
    public class Command_Clear : TerminalCommand {

        public override KeyValuePair<float, List<string>> RunCommand(TerminalUI console, string command) {
            console.Container.Clear();
            return new KeyValuePair<float, List<string>>();
        }
    }
}