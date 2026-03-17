using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItem : QuestReactionParent
{
    public override void ReactToQuest(Quest quest)
    {
        if (_quest == quest && quest.QuestState == _questState)
        {
            gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }
}
