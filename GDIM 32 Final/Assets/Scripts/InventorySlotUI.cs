using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;

    public void Bind(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.item.icon;

        countText.text = slot.count > 1 ? $"x{slot.count}" : "";
    }
}
