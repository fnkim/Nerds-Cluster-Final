using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    [SerializeField] private DialogueBubble bubble;

    private DialogueData _current;
    private int _index;

    public bool IsDialogueActive => _current != null;
    private void Awake()
    {
        Instance = this;
        bubble.HideDialogue();
    }
    public void StartDialogue(DialogueData asset)
    {
        _current = asset;
        _index = 0;
        ShowCurrentLine();
    }
    public void Advance()
    {
        if (_current == null) return;
        _index++;
        if (_index >= _current.lines.Length)
        {
            EndDialogue();
            return;
        }
        ShowCurrentLine();
    }
    public void EndDialogue()
    {
        _current = null;
        bubble.HideDialogue();
    }
    private void ShowCurrentLine()
    {
        bubble.ShowDialogue(_current.lines[_index]);
    }
}
