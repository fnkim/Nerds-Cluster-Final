using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text interactPromptText;

    private Transform _target;

    public void ShowPrompt(string text, Transform anchor)
    {
        interactPromptText.text = text;
        interactPromptText.gameObject.SetActive(true);
        _target = anchor;
    }

    public void HidePrompt()
    {
        interactPromptText.gameObject.SetActive(false);
        _target = null;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position);
        interactPromptText.transform.position = screenPos;
    }
}
