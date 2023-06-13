using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Insomnia{
	public abstract class EffectorBase : Interactable {

        #region Interactable Functions

        public sealed override bool OnInteractStart() {
            if(User == null)
                return true;

            Use(User);
            return true;
        }

        public sealed override void OnInteractEnd() { }


        #endregion

        protected abstract void Use(Interactor user);
    }
}
