using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T: Component{
    protected static T _instance = default(T);

    protected virtual void Awake() {
        if(_instance != null) {
            Destroy(gameObject);
            return;
        }
        else {
            _instance = gameObject.GetComponent<T>();
        }
    }
}
