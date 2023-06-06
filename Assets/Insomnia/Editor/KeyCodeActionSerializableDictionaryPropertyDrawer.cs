using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static TriggerInputActor;

[CustomPropertyDrawer(typeof(KeyCodeActionDictionary))]
public class KeyCodeActionSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
