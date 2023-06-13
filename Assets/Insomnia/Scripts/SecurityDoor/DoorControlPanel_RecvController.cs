using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class DoorControlPanel_RecvController : RecvControllerBase {

        #region RecvControllerBase Functions
        protected override void Awake() {
            base.Awake();
        }

        public override void StandbyInteract() {
            base.StandbyInteract();
        }
        public override void ReleaseInteract() {
            base.ReleaseInteract();
        }

        public override void OnInteractStartSuccess() {
            base.OnInteractStartSuccess();
        }

        public override void OnInteractEnd() {
            base.OnInteractEnd();
        }
        
        public override void InteractConditionSolved(object condition) {
            base.InteractConditionSolved(condition);
        }

        #endregion
    }
}