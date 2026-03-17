using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum QuestState{Null, NotStarted, Started, Failed, Succeeded, Ended}


[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public QuestState QuestState;
}
