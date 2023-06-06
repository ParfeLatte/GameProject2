using Insomnia;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInputActor : InputActor{
    [Serializable]
    public class KeyCodeActionDictionary : SerializableDictionary<KeyCode, UnityEvent> { }

    [SerializeField] private KeyCodeActionDictionary _keyCodes = new KeyCodeActionDictionary();
    private KeyCodeActionDictionary KeyCodes { get => _keyCodes; }

    public override sealed void KeyCheck() {
        if(IsEnabled == false)
            return;

        foreach(KeyCode key in _keyCodes.Keys) {
            if(Input.GetKeyDown(key))
                _keyCodes[key].Invoke();
        }
    }
}
