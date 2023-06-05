using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class MortalSingleton<T> : MonoBehaviour where T : Component {
        private static T _instance = default(T);
        public static T Instance {
            get {
                if(_instance == null)
                    _instance = FindObjectOfType<T>();

                return _instance;
            }
        }

        protected virtual void Awake() {
            if(_instance != null) {
                Destroy(gameObject);
                return;
            }
            else {
                _instance = gameObject.GetComponent<T>();
            }
        }

        protected virtual void OnDestroy() {
            _instance = null;
        }
    }
}