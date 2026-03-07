using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Talk";
    [SerializeField] private DialogueNode dialogue;
    public string PromptText => promptText;
    [SerializeField] private Transform promptAnchor;
    public Transform PromptAnchor => promptAnchor;
    [SerializeField] private Animator animator;

    public void Interact(PlayerInteractor interactor)
    {
        animator.SetBool("Talking", true); //Not completely fixed yet, I want to incorporate this into who's speaking. For now, it just plays the talk animation endlessly
        
        Debug.Log($"Interacting with NPC: {name}");
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
