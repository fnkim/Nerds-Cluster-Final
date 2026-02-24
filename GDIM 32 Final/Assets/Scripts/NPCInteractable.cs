using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Talk";
    public string PromptText => promptText;

    public void Interact()
    {
        Debug.Log($"Interacting with NPC: {name}");
    }
}
