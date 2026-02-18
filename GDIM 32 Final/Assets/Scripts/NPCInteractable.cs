using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractables
{
    // Start is called before the first frame update
    [SerializeField] private string prompt = "Press [E] to interact";

    public string Prompt => prompt;

    public void Interact()
    {
        // Replace with your dialogue, animation, etc.
        Debug.Log($"Talking to {gameObject.name}!");
    }
}
