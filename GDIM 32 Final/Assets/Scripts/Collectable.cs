using UnityEngine;

public enum BushType { Blueberry, Strawberry, Lemon }

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField] private BushType type;
    [SerializeField] private int amount = 1;
    [SerializeField] private string promptText = "[E] Pick up";
    public string PromptText => promptText;
    [SerializeField] private Transform promptAnchor;
    public Transform PromptAnchor => promptAnchor;
    public void Interact(PlayerInteractor interactor)
    {
        Debug.Log($"Collected: {type} x{amount}");
        Destroy(gameObject);
    }
}

