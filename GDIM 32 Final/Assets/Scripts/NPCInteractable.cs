using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Talk";
    [SerializeField] private DialogueAsset dialogue;
    public string PromptText => promptText;

    public void Interact(PlayerInteractor interactor)
    {
        Debug.Log($"Interacting with NPC: {name}");
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
