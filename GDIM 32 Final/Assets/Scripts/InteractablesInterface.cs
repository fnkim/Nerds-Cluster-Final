using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractables
{
    void Interact();
    string Prompt { get; }
}
