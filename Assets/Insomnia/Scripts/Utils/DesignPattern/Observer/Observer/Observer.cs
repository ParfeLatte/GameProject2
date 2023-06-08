using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insomnia {
    public abstract class Observer : MonoBehaviour{

        protected virtual void Start() { OnActivate(); }

        protected virtual void OnDestroy() { OnDeactivate(); }

        /// <summary>
        /// Subscribe Subject.
        /// </summary>
        public abstract void OnActivate();

        /// <summary>
        /// Unsubscribe Subject.
        /// </summary>
        public abstract void OnDeactivate();

        /// <summary>
        /// Get Notify from Subject
        /// </summary>
        /// <param name="noti"></param>
        public abstract void Notify(object noti);
    }
}

