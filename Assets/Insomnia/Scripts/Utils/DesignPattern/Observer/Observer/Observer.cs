using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insomnia {
    public abstract class Observer : MonoBehaviour{

        private void Start() => OnStart();

        private void OnDisable() => OnEnd();

        private void OnApplicationQuit() => OnEnd();

        /// <summary>
        /// Subscribe Subject.
        /// </summary>
        public abstract void OnStart();

        /// <summary>
        /// Unsubscribe Subject.
        /// </summary>
        public abstract void OnEnd();

        /// <summary>
        /// Get Notify from Subject
        /// </summary>
        /// <param name="noti"></param>
        public abstract void Notify(object noti);
    }
}

