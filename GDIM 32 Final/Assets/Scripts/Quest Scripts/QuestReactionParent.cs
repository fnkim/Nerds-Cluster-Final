using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReactionParent : MonoBehaviour
{
    public QuestState _questState;
    public Quest _quest;

    void Start()
    {
        DialogueManager.Instance.QuestChanged += ReactToQuest;
    }

    // Update is called once per frame
    public virtual void ReactToQuest(Quest quest)
    {
    }
}
