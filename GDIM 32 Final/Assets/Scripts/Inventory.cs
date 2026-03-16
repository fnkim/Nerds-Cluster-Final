using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    [SerializeField] private int slotCount = 12;

    private List<InventorySlot> _slots = new();

    public IReadOnlyList<InventorySlot> Slots => _slots;

    private void Awake()
    {
        for (int i = 0; i < slotCount; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    public void Add(ItemData item, int amount)
    {
        // Try to stack first
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty && slot.item == item)
            {
                slot.count += amount;
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        // Otherwise use empty slot
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty)
            {
                slot.item = item;
                slot.count = amount;
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        Debug.Log("Inventory Full");
    }

    public bool Has(ItemData item, int amount)
    {
        int total = 0;

        foreach (var slot in _slots)
        {
            if (slot.item == item)
                total += slot.count;
        }

        return total >= amount;
    }

    public bool Remove(ItemData item, int amount)
    {
        if (!Has(item, amount)) return false;

        foreach (var slot in _slots)
        {
            if (slot.item != item) continue;

            int removeAmount = Mathf.Min(slot.count, amount);
            slot.count -= removeAmount;
            amount -= removeAmount;

            if (slot.count <= 0)
                slot.Clear();

            if (amount <= 0)
                break;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }
    public InventorySlot GetSlot(int index)
    {
        if (index < 0 || index >= _slots.Count) return null;
        return _slots[index];
    }

    public bool RemoveFromSlot(int index, int amount)
    {
        if (index < 0 || index >= _slots.Count) return false;

        InventorySlot slot = _slots[index];
        if (slot == null || slot.IsEmpty || slot.count < amount) return false;

        slot.count -= amount;

        if (slot.count <= 0)
            slot.Clear();

        OnInventoryChanged?.Invoke();
        return true;
    }
}
