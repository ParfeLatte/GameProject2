using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public abstract class MortalSingleton<T> : SingletonBase<T> where T : Component {
        public static T Instance {
            get {
                if(_instance == null)
                    _instance = FindObjectOfType<T>();

                return _instance;
            }
        }

        protected void OnDestroy() {
            _instance = null;
        }
    }
}