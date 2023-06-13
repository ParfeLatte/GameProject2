using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName ="Command/Essential/Clear", fileName ="Command_Clear")]
    public class Command_Clear : CommandSO {

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal_Searchable terminal, string command) {
            terminal.UI.Container.Clear();
            yield return default;
        }
    }
}