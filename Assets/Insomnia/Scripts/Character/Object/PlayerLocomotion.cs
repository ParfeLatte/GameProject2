using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Insomnia {
    public class PlayerLocomotion : MonoBehaviour {
        #region Player Input
        [SerializeField] private KeyboardAxisActor _keyboardInput = null;

        #endregion

        #region Components
        private Rigidbody2D _rigid = null;


        #endregion

        private void Awake() {
            _keyboardInput = GetComponentInChildren<KeyboardAxisActor>();
            _rigid = GetComponent<Rigidbody2D>();
        }
    }
}