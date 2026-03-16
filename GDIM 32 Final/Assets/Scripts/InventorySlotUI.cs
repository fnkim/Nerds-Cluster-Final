using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject highlightObject;

    public void Bind(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.item.icon;
        countText.text = slot.count > 1 ? $"x{slot.count}" : "";
    }

    public void SetHighlighted(bool highlighted)
    {
        if (highlightObject != null)
            highlightObject.SetActive(highlighted);
    }
}
