using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/Dialogue Asset")]
public class DialogueNode : ScriptableObject
{
    
    public DialogueData[] _lines;
    public string[] _playerReplyOptions;

    public DialogueNode[] _npcReplies;
}