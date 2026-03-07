using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    [SerializeField] private DialogueBubble _dialogue;

    private DialogueNode _currentNode;

    private NextNode _nextNode;

    [SerializeField] PlayerInteractor interactor;

    


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

        } 
        
        //If the set quest state action has something in it
        else if (_currentNode._setQuestState._questStateToSet != QuestState.Null)
        {
            //Sets the _questToSet's Quest State to _questStateToSet
            _currentNode._setQuestState._questToSet.QuestState = _currentNode._setQuestState._questStateToSet;
            EndDialogue();

        } 
        //if there's an item in the inspector
        
        else if (_currentNode.item != null)
        {
            //grab the inventory
            Inventory inv = interactor.GetComponent<Inventory>();
            //add the item to the inventory
            inv.Add(_currentNode.item, 1);
            EndDialogue();
        }
        //if everything else is done
        else
        {
            //if there are any nodes in the current node's list of next nodes
            if (_currentNode._nextNode != null)
            {
                //cycle through the list of next nodes
                foreach(NextNode _nextNode in _currentNode._nextNode)
                {
                    //if one of them has a friendship check
                    if (_nextNode.friendshipCheck != null)
                    {
                        //there's probably a better way to do this but basically, this checks for if the friendship level is >= the friendship check's required friendship level
                        if (_nextNode.friendshipCheck._friendship.FriendshipLevel >= _nextNode.friendshipCheck._friendshipCondition)
                        {
                            _currentNode = _nextNode._nextDialogueNode;
                        }
                        else
                        {
                            return;
                        }
                        //

                    }
                }            
            }

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
        //Selects which node to go to. "nodeToGoTo" contains the next dialogue node and friendship variable modifiers
        NextNode nodeToGoTo = _currentNode._nextNode[option];

        //friendship variable
        FriendshipVariable friendshipVariable = nodeToGoTo.friendshipChange._friendship;

        //operation to change the friendship variable
        OperationType operation = nodeToGoTo.friendshipChange._operation;

        if (friendshipVariable != null)
        {
            switch (operation)
            {
                case OperationType.Null:
                    break;
                case OperationType.Add:
                    friendshipVariable.FriendshipLevel ++;
                    break;
                case OperationType.Subtract:
                    friendshipVariable.FriendshipLevel --;
                    break;
                default:
                    break;
                    
            }
        }


        _currentLine = 0;
        _waitingForPlayerResponse = false;

        _currentNode = nodeToGoTo._nextDialogueNode;

        Advance();
    }
}
