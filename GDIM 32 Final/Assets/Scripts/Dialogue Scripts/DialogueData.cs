using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Speaker {Squirrel, Witch, Traveler}



[Serializable]
public class DialogueData
{
    public Speaker _speaker;
    public Sprite _portrait;
    
    public string _dialogueText;
}
