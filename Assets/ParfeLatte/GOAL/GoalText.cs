using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoalText", menuName = "Scriptable Object/Goal")]
public class GoalText : ScriptableObject
{
    [TextArea]
    public string Goal;
}
