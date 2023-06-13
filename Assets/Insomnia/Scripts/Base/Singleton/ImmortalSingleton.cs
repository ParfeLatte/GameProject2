using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public abstract class ImmortalSingleton<T> : SingletonBase<T> where T : Component {
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

        protected override void Awake() {
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