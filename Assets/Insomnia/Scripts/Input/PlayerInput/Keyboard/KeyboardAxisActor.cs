using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class KeyboardAxisActor : AnalogInputActor {
        [SerializeField] private Vector2 m_inputVector = Vector2.zero;
        public Vector2 InputVector { get => m_inputVector; }

        public override void KeyCheck() {
            m_inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        }
    }
}