using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private InteractPromptUI promptUI;

    private IInteractable current;

    private void Update()
    {
        if (current != null && Input.GetKeyDown(interactKey))
        {
            current.Interact();
            promptUI.Hide();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            SetCurrent(interactable);
            return;
        }

        if (other.GetComponentInParent<IInteractable>() is IInteractable parentInteractable)
        {
            SetCurrent(parentInteractable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (current == null) return;

        var exiting = other.GetComponentInParent<IInteractable>();
        if (exiting != null && ReferenceEquals(exiting, current))
        {
            ClearCurrent();
        }
    }

    private void SetCurrent(IInteractable interactable)
    {
        current = interactable;
        promptUI.Show(interactable.PromptText);
    }

    private void ClearCurrent()
    {
        current = null;
        promptUI.Hide();
    }
}
