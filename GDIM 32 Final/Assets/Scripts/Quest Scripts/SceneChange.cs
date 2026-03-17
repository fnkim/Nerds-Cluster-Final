using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : QuestReactionParent
{
    // Update is called once per frame
    public override void ReactToQuest(Quest quest)
    {
        if (_quest == quest && quest.QuestState == _questState)
        {
            SceneManager.LoadScene("Start Cutscene");
        }
        else
        {
            return;
        }
    }
}
