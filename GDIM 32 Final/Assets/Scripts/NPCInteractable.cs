using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Talk";

  //This one sets up the list of quests that correspond with dialogue nodes.
    [SerializeField] private QuestToDialogue[] _questToDialogueList;

    [SerializeField] private Quest _currentQuest;

    private DialogueNode dialogue;
    public string PromptText => promptText;
    [SerializeField] private Transform promptAnchor;
    public Transform PromptAnchor => promptAnchor;
    [SerializeField] private Animator animator;


    public void Interact(PlayerInteractor interactor)
    {
        if (_currentQuest != null)
        {
            SelectDialogueNode(_currentQuest);
        }
        else
        {
            Debug.Log("There's no current quest dude");
        }

        
        Debug.Log($"Interacting with NPC: {name}");
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    void SelectDialogueNode(Quest _currentQuest)
    {
        foreach (QuestToDialogue questInList in _questToDialogueList)
        {
            //It's 5 am rn and this is frying my brain but basically this is kinda how I'm logicking it out:
            //There's a list of quests in the NPC interactable that contains serializable class: Quest to Dialogue
            //Quest to Dialogue contains the QuestState enum as questStateCondition and a dialogueNode
            //There is also a ScriptableObject called Quest, which contains a QuestState enum, which is selectable in this script's inspector as _currentQuest
            //In this script, NPC Interactable, there is an inspector editable list of QuestToDialogues: _questToDialogueList
            // This foreach statement cycles through the list and matches it against the enum of _currentQuest
            //It then sets the dialogue node "dialogue" as the DialogueNode in the selected QuestToDialogue



            //For example, let's say the currentquest is SquirrelQuest
            //In the _questToDialogueList, there's: 0. questStateCondition - NotStarted and dialogueToPlay - Squirrel 1
            // 1. questStateCondition - Started and dialogueToPlay - Squirrel 2, etc
            // if the state of the SquirrelQuest is currently Started, then it would grab index 1 in _questToDialogueList
            // From there, it would set dialogue to Squirrel 2
            if (questInList.questStateCondition == _currentQuest.QuestState)
            {
                dialogue = questInList.dialogueToPlay;
            }
        }
    }


}
