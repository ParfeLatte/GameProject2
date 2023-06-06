using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName ="Command/Essential/Clear", fileName ="Command_Clear")]
    public class Command_Clear : TerminalCommand {

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal terminal, string command) {
            terminal.UI.Container.Clear();
            yield return default;
        }
    }
}