using UnityEngine;

public enum BushType { Blueberry, Strawberry, Lemon }

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField] private BushType type;
    [SerializeField] private int amount = 1;

    [SerializeField] private string promptText = "[E] Pick up";
    public string PromptText => _collected ? "" : promptText;

    [SerializeField] private Transform promptAnchor;
    public Transform PromptAnchor => promptAnchor;

    [SerializeField] private GameObject visualToDestroy;

    private bool _collected;

    public void Interact(PlayerInteractor interactor)
    {
        if (_collected) return;
        _collected = true;

        Debug.Log($"Collected: {type} x{amount}");

        if (visualToDestroy != null)
            Destroy(visualToDestroy);

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(this);
    }
}

