using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    [SerializeField] private DialogueBubble _dialogue;

    private DialogueNode _currentNode;
    private int _currentLine;

    public bool IsDialogueActive => _currentNode != null;
    
    private bool _waitingForPlayerResponse;
    
    [SerializeField] private Image _portraitUI;





    private void Awake()
    {
        Instance = this;
        _dialogue.HideDialogue();
    }






    public void StartDialogue(DialogueNode asset)
    {
        _currentNode = asset;
        //sets current line to -1 so that when it advances it gets set to 0
        _currentLine = 0;
        Advance();
    }

    private void Update ()
    {
        if (_currentNode == null) return;
        
        //checks if the typewriter effect is going
        if (!_dialogue.IsTyping)
        {
            // if space or mouse button click has been pressed once
            if(!_waitingForPlayerResponse && Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Advance();
            }
        }
        // if the typewriter effect has stopped
        else
        {
            //if space or mousebutton click is being held
            if(!_waitingForPlayerResponse && Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                //speeds up typewriter effect
                _dialogue.speedUpText = true;
            }
            else
            {
                _dialogue.speedUpText = false;
            }            
        }






    }


    public void Advance()
    {
    /*Basically there is a ScriptableObject called DialogueNode which contains a list of DialogueData, which
    is a class that contains the dialogue text and the speaker enum. The line below grabs the Dialogue Data
    info from the current node and current line.
    */



        if (_currentNode == null) return;

        if (_currentLine < _currentNode._lines.Length)
        {
            DialogueData _dialogueData = _currentNode._lines[_currentLine];


            //this grabs the enum for who's the speaker from the dialogue data in the dialoguenode list
            var _currentSpeaker = _dialogueData._speaker;
            //I'm gonna implement name stuff


            _dialogue.ShowDialogue(_dialogueData._dialogueText);
            if (_dialogueData._portrait != null)
            {
                _portraitUI.sprite = _dialogueData._portrait;
            }
            else
            {
                _portraitUI.sprite = null;
            }
            _currentLine++;
        }
        else if (_currentNode._playerReplyOptions != null && _currentNode._playerReplyOptions.Length > 0)
        {
            //show player options
            _waitingForPlayerResponse = true;
            _dialogue.ShowPlayerOptions(_currentNode._playerReplyOptions);

        } else
        {
            //if no more lines left, close UI
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        _currentNode = null;
        _currentLine = 0;
        _waitingForPlayerResponse = false;
        _dialogue.HideDialogue();
    }
    public void SelectedOption(int option)
    {
        _currentLine = 0;
        _waitingForPlayerResponse = false;

        _currentNode = _currentNode._npcReplies[option];
        Advance();
    }
}
