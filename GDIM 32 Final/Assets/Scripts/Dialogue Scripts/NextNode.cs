using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperationType{Null, Add, Subtract}

[Serializable]
public class NextNode
{
    [Header("Friendship Condition")]
    [Header("Leave blank if not applicable.")]
    public FriendshipCheck friendshipCheck;


    [Header("Friendship Level Change")]
    [Header("Leave blank if not applicable.")]
    public FriendshipChange friendshipChange;

    [Header("Extra Lines at the end")]
    [Header("Leave blank if not applicable.")]
    public DialogueData[] extraLines;

    [Header("Choose the next dialogue node.")]
    
    public DialogueNode _nextDialogueNode;

}
