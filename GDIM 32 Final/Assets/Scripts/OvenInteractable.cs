using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "[E] Use";
    [SerializeField] private Transform promptAnchor;
    [SerializeField] private OvenMenuController ovenMenuController;

    public string PromptText => promptText;
    public Transform PromptAnchor => promptAnchor;

    public void Interact(PlayerInteractor interactor)
    {
        if (ovenMenuController != null)
            ovenMenuController.OpenMenu();
    }
}
