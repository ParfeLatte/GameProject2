using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class ImmortalSingleton<T> : MonoBehaviour where T : Component {
        protected static T _instance = default(T);
        public static T Instance {
            get {
                if(_instance == null) {
                    _instance = FindObjectOfType<T>();

                    if(_instance == null) {
                        GameObject go = new GameObject() { name = typeof(T).Name };
                        _instance = go.AddComponent<T>();
                    }
                }

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
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}