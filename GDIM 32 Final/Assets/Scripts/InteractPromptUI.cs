using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text interactPromptText;
    [SerializeField] private GameObject travelerDialogue;

    public void ShowPrompt(string text)
    {
        interactPromptText.text = text;
        interactPromptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        interactPromptText.gameObject.SetActive(false);
    }
    public void ShowDialogue()
    {
        travelerDialogue.gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        travelerDialogue.gameObject.SetActive(false);
    }
}
