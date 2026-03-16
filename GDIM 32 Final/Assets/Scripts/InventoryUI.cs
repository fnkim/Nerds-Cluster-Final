using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform gridParent;
    [SerializeField] private InventorySlotUI slotPrefab;

    private InventorySlotUI[] _slotUIs;

    private void Start()
    {
        if (inventory == null) return;

        if (_slotUIs == null || _slotUIs.Length != inventory.Slots.Count)
        {
            RebuildSlots();
        }

        inventory.OnInventoryChanged -= Refresh;
        inventory.OnInventoryChanged += Refresh;

        Refresh();
    }

    public void SetInventory(Inventory newInventory)
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;

        inventory = newInventory;

        if (inventory == null) return;

        if (_slotUIs == null || _slotUIs.Length != inventory.Slots.Count)
        {
            RebuildSlots();
        }

        inventory.OnInventoryChanged -= Refresh;
        inventory.OnInventoryChanged += Refresh;

        Refresh();
    }

    private void RebuildSlots()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        _slotUIs = new InventorySlotUI[inventory.Slots.Count];

        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            var ui = Instantiate(slotPrefab, gridParent);
            _slotUIs[i] = ui;
        }
    }

    public void Refresh()
    {
        if (inventory == null || _slotUIs == null) return;

        for (int i = 0; i < _slotUIs.Length; i++)
        {
            _slotUIs[i].Bind(inventory.Slots[i]);
        }
    }

    public RectTransform GetSlotRect(int index)
    {
        if (_slotUIs == null) return null;
        if (index < 0 || index >= _slotUIs.Length) return null;

        return _slotUIs[index].GetComponent<RectTransform>();
    }

    public void SetHighlight(int index, bool highlighted)
    {
        if (_slotUIs == null) return;
        if (index < 0 || index >= _slotUIs.Length) return;

        _slotUIs[index].SetHighlighted(highlighted);
    }

    public int SlotCount
    {
        get
        {
            if (_slotUIs == null) return 0;
            return _slotUIs.Length;
        }
    }
}
