using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string PromptText { get; }
    Transform PromptAnchor { get; }
    void Interact(PlayerInteractor interactor);
}
