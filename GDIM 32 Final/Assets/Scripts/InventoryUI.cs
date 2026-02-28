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
        int count = inventory.Slots.Count;
        _slotUIs = new InventorySlotUI[count];

        for (int i = 0; i < count; i++)
        {
            var ui = Instantiate(slotPrefab, gridParent);
            _slotUIs[i] = ui;
        }

        inventory.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < _slotUIs.Length; i++)
        {
            _slotUIs[i].Bind(inventory.Slots[i]);
        }
    }
}
