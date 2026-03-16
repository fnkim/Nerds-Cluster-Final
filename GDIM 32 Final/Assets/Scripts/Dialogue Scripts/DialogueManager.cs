using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    
    public static DialogueManager Instance { get; private set; }
    [SerializeField] private DialogueBubble _dialogue;

    [SerializeField] PlayerInteractor interactor;

    public bool IsDialogueActive => _currentNode != null;
       
    [SerializeField] private Image _portraitUI;



// VARIABLES THAT KEEP TRACK OF DIALOGUE STUFF
    // Sets the current dialogue node
    private DialogueNode _currentNode;


    // variable that holds the NextNode
    private NextNode _nodeToGoTo;

    // Bool that checks whether the starting dialogue lines are over or not
    private bool _dialogueOver;

    // integer for current line
    private int _currentLine;

    //variable for length of the current array of lines
    private int _lengthOfArray;

    // variable for the current DialogueData
    private DialogueData _currentData;

    // bool for whether it is waiting for the player to click a choice
    private bool _waitingForPlayerResponse;



    private void Awake()
    {
        Instance = this;
        _dialogue.HideDialogue();
    }



    public void StartDialogue(DialogueNode asset)
    {
        _currentNode = asset;
        _currentLine = 0;
        
        //sets up the node stuff with the current node's data.
        // read the comments i left on SetupNode()
        SetupNode(_currentNode);
        Debug.Log(_currentNode);
        Debug.Log(_currentData);
        //advances with current data
        Advance(_currentData);

    }

    private void Update ()
    {

        if (_currentNode == null) return;
        
        
        TypeWriter();

    }


    private void TypeWriter()
    {

        
        // if the typing effect has stopped
        if (!_dialogue.IsTyping)
        {
            // if space or mouse button click has been pressed once
            if(!_waitingForPlayerResponse && Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (_dialogueOver)
                {
                    // if the current line int hasn't gone over the array length and if the current node has lines
                    if (_currentLine < _lengthOfArray)
                    {
                        _currentData = _nodeToGoTo.extraLines[_currentLine];
                    }
                    else
                    {
                        _currentData = null;
                    }
                }
                //if the starting lines are still going
                else
                {
                    // if the current line int hasn't gone over the array length and if the current node has lines
                    if (_currentLine < _lengthOfArray)
                    {
                        //sets currentData to the current node's current line (which is element 0)
                        _currentData = _currentNode._lines[_currentLine];
                    }
                    else
                    {
                        _currentData = null;
                    }
                }

                Advance(_currentData);
            }
        }
        // if the typewriter effect is still going
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


    public void Advance(DialogueData _dialogueData)
    {
        // if there's no currentNode, return
        if (_currentNode == null) return;

    // CHANGES QUESTS AND ITEMS

        //if there's an item in the inspector
        if (_currentNode.item != null)
        {
            //grab the inventory
            Inventory inv = interactor.GetComponent<Inventory>();
            //add the item to the inventory
            inv.Add(_currentNode.item, 1);
        }

        //If the set quest state action has something in it
        if (_currentNode._setQuestState._questStateToSet != QuestState.Null)
        {
            //Sets the _questToSet's Quest State to _questStateToSet
            _currentNode._setQuestState._questToSet.QuestState = _currentNode._setQuestState._questStateToSet;

        } 



    // ADVANCES DIALOGUE



        //if there's nothing in the current data, advance after lines
        if (_currentData == null)
        {
            AdvanceAfterLines();
        }
        //if the current line int is less than _lengthOfArray
        else if(_currentLine < _lengthOfArray)
        {
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
        else
        {
            AdvanceAfterLines();
        }

    }


    private void AdvanceAfterLines()
    {
        //basically, if the dialogue node hasn't progressed onto the extra lines yet
        if(_dialogueOver == false)
        {
            //if there are reply options
            if (_currentNode._playerReplyOptions.Length != 0)
            {
                //show player options
                _waitingForPlayerResponse = true;
                _dialogue.ShowPlayerOptions(_currentNode._playerReplyOptions);

            }
            //if there are next nodes
            else if (_currentNode._nextNode.Length != 0)            
            {
                //cycle through the list of next nodes
                bool _friendshipConditionMet = false;

                for (int i = 0; i < _currentNode._nextNode.Length; i++)
                {
                    Debug.Log(i);
                    //if the index's friendship check has something in it
                    if (_currentNode._nextNode[i].friendshipCheck._friendship != null)
                    {
                        // checks for if current friendship level is >= the  required friendship level
                        if (_currentNode._nextNode[i].friendshipCheck._friendship.FriendshipLevel >= _currentNode._nextNode[i].friendshipCheck._friendshipCondition)
                        {
                            _friendshipConditionMet = true;
                            Debug.Log(_currentNode._nextNode[i].friendshipCheck._friendship.FriendshipLevel);
                            Debug.Log(_currentNode._nextNode[i].friendshipCheck._friendshipCondition);
                            SelectedOption(i);
                            break;
                        }
                    }           
                }
                if (_friendshipConditionMet == false)
                {
                    EndDialogue();
                }
            }
            //if there aren't next nodes
            else
            {
                EndDialogue();
            }
        }
        
        //advance to the next node after the extra lines
        else
        {
            // if the node to go to's next node has stuff in it
            if (_nodeToGoTo._nextDialogueNode != null)
            {
                //sets the current node to the next node
                _currentNode = _nodeToGoTo._nextDialogueNode;
                //sets up the next node's array stuff and everything
                SetupNode(_currentNode); 
                //sets the node to go to variable to null to refresh it
                _nodeToGoTo = null;
                //advances with the current data
                Advance(_currentData);
            }
            // if there is no next dialogue node
            else
            {
                EndDialogue();
            }

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
        // sets variable to say the dialogue is over
        _dialogueOver = true;

        _waitingForPlayerResponse = false;
        
        //Selects which node to go to. "nodeToGoTo" contains the next dialogue node and friendship variable modifiers
        _nodeToGoTo = _currentNode._nextNode[option];

        //friendship variable
        FriendshipVariable friendshipVariable = _nodeToGoTo.friendshipChange._friendship;

        //operation to change the friendship variable
        OperationType operation = _nodeToGoTo.friendshipChange._operation;

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

        // set current line int to 0
        _currentLine = 0;
        

        //if the node to go to has extra lines
        if (_nodeToGoTo.extraLines.Length != 0)
        {
            _lengthOfArray = _nodeToGoTo.extraLines.Length;
            // set current dialogue data to the next node to go to's 0th element extra line
            _currentData = _nodeToGoTo.extraLines[_currentLine];            
        }
        //if the node to go to doesn't have extra lines
        else
        {
            //set current node to the next dialogue node of _nodeToGoTo
            _currentNode = _nodeToGoTo._nextDialogueNode;
            SetupNode(_currentNode);
        }
        
        //advance line with the current data
        Advance(_currentData);
    }


    //sets up the next node. This means setting up the length of array and current data
    private void SetupNode(DialogueNode node)
    {
        _dialogueOver = false;
        // if the current node's lines array has more than 0 elements (basically, if it has literally anything in it)
        if (node._lines.Length != 0)
        {
            _currentLine = 0;
            //sets up length of array
            _lengthOfArray = node._lines.Length;
            //sets currentData (which is element 0) of _currentNode
            _currentData = node._lines[_currentLine];
            //turn off dialogue over to start over
        }
        // however, if the current node's lines array has nothing in it
        else
        {
            // there is no current data. this means when the code advances, it immediately moves onto the advance after lines part
            _currentData = null;
        }
    }
}


