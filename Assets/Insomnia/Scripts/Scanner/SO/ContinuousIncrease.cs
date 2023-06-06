using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Scanner/Action/ContinuousIncrease", fileName = "ContinuousIncrease")]
    public class ContinuousIncrease : ScanAction {
        public override void Calculate(Scanner scanner, float amountPerSec) {
            if(scanner.IsTriggered == false)
                scanner.Progress = 0;

            scanner.Progress += amountPerSec * Time.deltaTime;
        }
    }
}
