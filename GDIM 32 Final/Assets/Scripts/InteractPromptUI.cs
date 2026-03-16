using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text interactPromptText;

    [SerializeField] private GameObject interactPromptBubble;

    private Transform _target;

    public void ShowPrompt(Transform anchor)
    {
        interactPromptText.text = "[E]";
        interactPromptBubble.SetActive(true);
        _target = anchor;
    }

    public void HidePrompt()
    {
        interactPromptBubble.SetActive(false);
        _target = null;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position);
        interactPromptBubble.transform.position = screenPos;

        if (DialogueManager.Instance.IsDialogueActive)
        {
            HidePrompt();
        }



    }
}
