using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Asset")]
public class DialogueData : ScriptableObject
{
    public string speakerName;
    [TextArea(2, 6)]
    public string[] lines;
}
