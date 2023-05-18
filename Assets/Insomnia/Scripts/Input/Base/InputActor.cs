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
        /// �� Update���� �Է°��� üũ�ϴ� �Լ�.
        /// base.KeyCheck ȣ�� �� KeyCodes Dictionary�� ����� Ű���� GetKeyDown���� üũ�Ͽ�, �Է� �� ����� �Լ��� ȣ��.
        /// </summary>
        public abstract void KeyCheck();
        public virtual void ListenerEnabled() { _isEnabled = true; }
        public virtual void ListenerDisabled() { _isEnabled = false; }
    }
}