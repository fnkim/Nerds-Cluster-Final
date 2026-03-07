using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/Dialogue Asset")]
public class DialogueNode : ScriptableObject
{
    
    public DialogueData[] _lines;
    public string[] _playerReplyOptions;
    public NextNode[] _nextNode;

    [Header("Manually sets the next quest state")]
    [Header("Leave blank if not applicable.")]
    public SetQuestState _setQuestState;

    [Header("Adds an item to inventory")]
    [Header("Leave blank if not applicable.")]
    public ItemData item;



}