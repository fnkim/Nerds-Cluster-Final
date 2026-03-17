using UnityEngine;

public enum BushType { Blueberry, Strawberry, Lemon }

public class Collectable : MonoBehaviour, IInteractable
{
    public ItemData item;
    [SerializeField] private int amount = 1;

    [SerializeField] private string promptText = "[E] Pick up";
    public string PromptText => _collected ? "" : promptText;

    [SerializeField] private Transform promptAnchor;
    public Transform PromptAnchor => promptAnchor;

    [SerializeField] private GameObject visualToRemove;

    [SerializeField] private Witch Player;

    [Header("Sets Quest State upon collecting")]
    [Header("Leave alone if not applicable")]
    [SerializeField] private SetQuestState _setQuestState;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        Player = playerObj.GetComponent<Witch>();
    }

    private bool _collected;

    public void Interact(PlayerInteractor interactor)
    {
        if (_collected) return;
        _collected = true;
        

        // Find inventory on player
        Inventory inv = interactor.GetComponent<Inventory>();
        if (inv == null)
        {
            Debug.LogError("PlayerInteractor has no Inventory on the player!");
            return;
        }

        inv.Add(item, amount);

        //If the set quest state action has something in it
        if (_setQuestState._questStateToSet != QuestState.Null)
        {
            //Sets the _questToSet's Quest State to _questStateToSet
            _setQuestState._questToSet.QuestState = _setQuestState._questStateToSet;

        } 


        // Remove visuals
        if (visualToRemove != null)
            Destroy(visualToRemove);

        // Disable colliders so it can't be targeted anymore
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;


        // Remove interactable script itself (extra safety)
        Destroy(this);
    }
}

