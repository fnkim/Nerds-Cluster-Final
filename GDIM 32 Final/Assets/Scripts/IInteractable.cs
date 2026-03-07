using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    //Added gameobject to IInteractable so the player interactor can reference it.
    //Nothing needs to be set for the children since they alr inherit from monobehavior.
    // I'm tired so idk if this might have exceptions. Sorry if we gotta change it later.
    GameObject gameObject { get; }
    string PromptText { get; }
    Transform PromptAnchor { get; }
    void Interact(PlayerInteractor interactor);
}
