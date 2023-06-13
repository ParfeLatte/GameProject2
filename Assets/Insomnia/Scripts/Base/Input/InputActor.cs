using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Insomnia {
    public abstract class InputActor : MonoBehaviour {
        [SerializeField] private bool _isEnabled = false;
        public bool IsEnabled { get => _isEnabled; }

        /// <summary>
        /// 매 Update마다 입력값을 체크하는 함수.
        /// base.KeyCheck 호출 시 KeyCodes Dictionary에 저장된 키들을 GetKeyDown으로 체크하여, 입력 시 연결된 함수를 호출.
        /// </summary>
        public abstract void KeyCheck();
        public virtual void ListenerEnabled() { _isEnabled = true; }
        public virtual void ListenerDisabled() { _isEnabled = false; }
    }
}