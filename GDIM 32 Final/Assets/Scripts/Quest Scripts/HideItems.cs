using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideItems : QuestReactionParent
{

    // Update is called once per frame
    public override void ReactToQuest(Quest quest)
    {
        //if the quest ScriptableObject, _quest, in this script == the quest ScriptableObject that's being passed through
        // and if the questState that's being passed through == _questState on this script
        // hide the gameobject
        if (_quest == quest && quest.QuestState == _questState)
        {
            gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
