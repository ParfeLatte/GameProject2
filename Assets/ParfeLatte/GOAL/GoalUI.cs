using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GoalUI : MonoBehaviour
{
    public GoalText StageGoal;
    public TMP_Text MainGoal;

    // Start is called before the first frame update
    void Start()
    {
        MainGoal.text = StageGoal.Goal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
