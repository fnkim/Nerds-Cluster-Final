using UnityEngine;

public enum BushType { Blueberry, Strawberry, Lemon }

public class BushCollectable : MonoBehaviour
{
    [SerializeField] private BushType type;
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
       Debug.Log($"Collected: {type} x{amount}");
;

        if (!other.CompareTag("Player")) return;

        gameObject.SetActive(false);
    }
}

