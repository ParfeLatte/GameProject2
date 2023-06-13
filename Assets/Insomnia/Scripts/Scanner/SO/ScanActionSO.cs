using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public abstract class ScanActionSO : ScriptableObject {
        public abstract void Calculate(Scanner scanner, float amountPerSec);
    }
}