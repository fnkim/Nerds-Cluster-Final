using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private float sphereRadius = 0.4f;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private InteractPromptUI promptUI;
    [SerializeField] private bool drawDebugGizmos = true;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject pressTabPanel;

    private IInteractable _currentTarget;
    public static PlayerInteractor Instance {get; private set; }
    public Witch Player {get; set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        GameObject playerObj = GameObject.FindWithTag("Player");
        Player = playerObj.GetComponent<Witch>();
    }
    private void Update()
    {
        if (rayOrigin == null || promptUI == null)
            return;

        UpdateRaycastTarget();

        if (Input.GetKeyDown(interactKey))
        {

            if (_currentTarget != null && !DialogueManager.Instance.IsDialogueActive)
            {
                _currentTarget.Interact(this);
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            Time.timeScale = inventoryPanel.activeSelf ? 0f : 1f;
            pressTabPanel.SetActive(!pressTabPanel.activeSelf);
        }
    }

    private void UpdateRaycastTarget()
    {
        if (rayOrigin == null) return;

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        RaycastHit[] hit = Physics.SphereCastAll(
            ray,
            sphereRadius,
            interactDistance,
            interactLayerMask,
            QueryTriggerInteraction.Ignore
        );

        IInteractable newTarget = null;
        float bestDist = float.MaxValue;

        foreach (var h in hit)
        {
            var interactable = h.collider.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;

            if (h.distance < bestDist)
            {
                bestDist = h.distance;
                newTarget = interactable;
            }
        }

        if (newTarget == _currentTarget) return;

        _currentTarget = newTarget;

        if (_currentTarget != null)
            promptUI.ShowPrompt(_currentTarget.PromptText, _currentTarget.PromptAnchor);
        else
        {
            promptUI.HidePrompt();
            DialogueManager.Instance.EndDialogue();
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawDebugGizmos) return;
        if (rayOrigin == null) return;

        Gizmos.color = Color.cyan;

        Vector3 start = rayOrigin.position;
        Vector3 end = start + rayOrigin.forward * interactDistance;

        Gizmos.DrawWireSphere(start, sphereRadius);
        Gizmos.DrawWireSphere(end, sphereRadius);

        Gizmos.DrawLine(start + rayOrigin.right * sphereRadius, end + rayOrigin.right * sphereRadius);
        Gizmos.DrawLine(start - rayOrigin.right * sphereRadius, end - rayOrigin.right * sphereRadius);
        Gizmos.DrawLine(start + rayOrigin.up * sphereRadius, end + rayOrigin.up * sphereRadius);
        Gizmos.DrawLine(start - rayOrigin.up * sphereRadius, end - rayOrigin.up * sphereRadius);
    }
}
