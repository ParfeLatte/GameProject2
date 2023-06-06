using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Scanner/Action/SimpleIncrease", fileName = "SimpleIncrease")]
    public class SimpleIncrease : ScanAction {
        public override void Calculate(Scanner scanner, float amountPerSec) {
            if(scanner.IsTriggered == false)
                return;

            scanner.Progress += amountPerSec * Time.deltaTime;
        }
    }
}