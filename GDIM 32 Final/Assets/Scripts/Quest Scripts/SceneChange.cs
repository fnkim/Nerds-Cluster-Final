using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : QuestReactionParent
{
    public override void ReactToQuest(Quest quest)
    {
        if (_quest == quest && quest.QuestState == _questState)
        {
            SceneManager.LoadScene("End Scene");
        }
        else
        {
            return;
        }
    }
}
