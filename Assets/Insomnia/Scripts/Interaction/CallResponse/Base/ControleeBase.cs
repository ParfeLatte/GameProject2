using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public abstract class ControleeBase : MonoBehaviour {
        [Header("ReactorBase: Status")]
        [SerializeField] protected bool m_isReacting = false;
        public abstract void StandbyInteract();

        public abstract void ReleaseInteract();

        public abstract bool OnInteractStart(object request);

        public abstract bool OnInteractEnd();
    }
}