using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Command/Essential/Exit", fileName = "Command_Exit")]
    public class Command_Exit : CommandSO {

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal_Searchable terminal, string command) {
            terminal.Interactable.OnInteractEnd();

            yield return default;
        }
    }
}