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

    private IInteractable _currentTarget;

    private void Update()
    {
        if (rayOrigin == null || promptUI == null)
            return;

        UpdateRaycastTarget();

        if (Input.GetKeyDown(interactKey))
        {
            if (DialogueManager.Instance.IsDialogueActive)
            {
                DialogueManager.Instance.Advance();
                return;
            }

            if (_currentTarget != null)
            {
                _currentTarget.Interact(this);
                return;
            }
        }
    }

    private void UpdateRaycastTarget()
    {
        if (rayOrigin == null) return;

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        RaycastHit[] hits = Physics.SphereCastAll(
            ray,
            sphereRadius,
            interactDistance,
            interactLayerMask,
            QueryTriggerInteraction.Ignore
        );

        IInteractable newTarget = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
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
            promptUI.ShowPrompt(_currentTarget.PromptText);
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
