using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName = "Scanner/Action/SignedIncrease", fileName = "SignedIncrease")]
    public class SignedIncrease : ScanAction {
        public override void Calculate(Scanner scanner, float amountPerSec) {
            scanner.Progress += ( amountPerSec * (scanner.IsTriggered ? 1 : -1) ) * Time.deltaTime;
        }
    }
}