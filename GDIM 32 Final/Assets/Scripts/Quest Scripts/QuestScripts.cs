using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestToDialogue
{
    public QuestState questStateCondition;

    public DialogueNode dialogueToPlay;

}

[Serializable]
public class SetQuestState
{
    public QuestState _questStateToSet;
    public Quest _questToSet;
}


[Serializable]
public class FriendshipCheck
{
    public FriendshipVariable _friendship;
    public int _friendshipCondition;
    
}

[Serializable]
public class FriendshipChange
{
    public FriendshipVariable _friendship;
    public OperationType _operation;

    
}