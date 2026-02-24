using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;

    public void Show(string text)
    {
        promptText.text = text;
        promptText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        promptText.gameObject.SetActive(false);
    }
}
