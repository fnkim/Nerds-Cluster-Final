using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string PromptText { get; }
    void Interact(PlayerInteractor interactor);
}
