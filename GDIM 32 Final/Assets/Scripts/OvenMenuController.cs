using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OvenMenuController : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject ovenMenuRoot;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Inventory ovenInventory;

    [Header("Inventory UIs")]
    [SerializeField] private InventoryUI normalPlayerInventoryUI;
    [SerializeField] private InventoryUI ovenPlayerInventoryUI;
    [SerializeField] private InventoryUI ovenInventoryUI;

    [Header("Bake UI")]
    [SerializeField] private Button bakeButton;

    [Header("Recipe")]
    [SerializeField] private RecipeData recipe;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bakeCompleteSfx;

    [Header("Grid Settings")]
    [SerializeField] private int playerGridColumns = 4;
    [SerializeField] private int ovenGridColumns = 3;

    private bool _isOpen;
    private bool _isBaking;
    private bool _selectingPlayerSide = true;
    private int _playerIndex;
    private int _ovenIndex;

    public bool IsOpen => _isOpen;

    private void Start()
    {
        if (ovenMenuRoot != null)
            ovenMenuRoot.SetActive(false);

        if (bakeButton != null)
            bakeButton.onClick.AddListener(OnBakePressed);

        if (normalPlayerInventoryUI != null)
            normalPlayerInventoryUI.SetInventory(playerInventory);

        if (ovenPlayerInventoryUI != null)
            ovenPlayerInventoryUI.SetInventory(playerInventory);

        if (ovenInventoryUI != null)
            ovenInventoryUI.SetInventory(ovenInventory);

        ClearAllHighlights();
    }

    private void Update()
    {
        if (!_isOpen || _isBaking)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
            return;
        }

        HandleNavigation();

        if (Input.GetKeyDown(KeyCode.T))
            TransferSelectedItem();
    }

    public void OpenMenu()
    {
        if (_isOpen) return;

        _isOpen = true;
        _isBaking = false;
        _selectingPlayerSide = true;
        _playerIndex = 0;
        _ovenIndex = 0;

        if (ovenMenuRoot != null)
            ovenMenuRoot.SetActive(true);

        if (normalPlayerInventoryUI != null)
            normalPlayerInventoryUI.Refresh();

        if (ovenPlayerInventoryUI != null)
            ovenPlayerInventoryUI.Refresh();

        if (ovenInventoryUI != null)
            ovenInventoryUI.Refresh();

        Time.timeScale = 0f;
        UpdateHighlights();
    }

    public void CloseMenu()
    {
        if (!_isOpen) return;

        _isOpen = false;

        if (ovenMenuRoot != null)
            ovenMenuRoot.SetActive(false);

        ClearAllHighlights();
        Time.timeScale = 1f;
    }

    private void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.W))
            MoveSelectionVertical(-1);

        if (Input.GetKeyDown(KeyCode.S))
            MoveSelectionVertical(1);

        if (Input.GetKeyDown(KeyCode.A))
            MoveSelectionHorizontal(-1);

        if (Input.GetKeyDown(KeyCode.D))
            MoveSelectionHorizontal(1);
    }

    private void MoveSelectionVertical(int direction)
    {
        if (_selectingPlayerSide)
        {
            int max = ovenPlayerInventoryUI != null ? ovenPlayerInventoryUI.SlotCount : 0;
            int newIndex = _playerIndex + direction * playerGridColumns;

            if (newIndex >= 0 && newIndex < max)
                _playerIndex = newIndex;
        }
        else
        {
            int max = ovenInventoryUI != null ? ovenInventoryUI.SlotCount : 0;
            int newIndex = _ovenIndex + direction * ovenGridColumns;

            if (newIndex >= 0 && newIndex < max)
                _ovenIndex = newIndex;
        }

        UpdateHighlights();
    }

    private void MoveSelectionHorizontal(int direction)
    {
        if (_selectingPlayerSide)
        {
            int max = ovenPlayerInventoryUI != null ? ovenPlayerInventoryUI.SlotCount : 0;
            int col = _playerIndex % playerGridColumns;

            if (direction < 0)
            {
                if (col > 0)
                    _playerIndex--;
            }
            else
            {
                if (col < playerGridColumns - 1 && _playerIndex + 1 < max)
                    _playerIndex++;
                else
                    _selectingPlayerSide = false;
            }
        }
        else
        {
            int max = ovenInventoryUI != null ? ovenInventoryUI.SlotCount : 0;
            int col = _ovenIndex % ovenGridColumns;

            if (direction > 0)
            {
                if (col < ovenGridColumns - 1 && _ovenIndex + 1 < max)
                    _ovenIndex++;
            }
            else
            {
                if (col > 0)
                    _ovenIndex--;
                else
                    _selectingPlayerSide = true;
            }
        }

        UpdateHighlights();
    }

    private void TransferSelectedItem()
    {
        if (_selectingPlayerSide)
            TransferToOven();
        else
            TransferToPlayer();
    }

    private void TransferToOven()
    {
        InventorySlot slot = playerInventory.GetSlot(_playerIndex);
        if (slot == null || slot.IsEmpty) return;

        ItemData item = slot.item;

        if (playerInventory.RemoveFromSlot(_playerIndex, 1))
        {
            ovenInventory.Add(item, 1);
            RefreshAllInventoryViews();
            UpdateHighlights();
        }
    }

    private void TransferToPlayer()
    {
        InventorySlot slot = ovenInventory.GetSlot(_ovenIndex);
        if (slot == null || slot.IsEmpty) return;

        ItemData item = slot.item;

        if (ovenInventory.RemoveFromSlot(_ovenIndex, 1))
        {
            playerInventory.Add(item, 1);
            RefreshAllInventoryViews();
            UpdateHighlights();
        }
    }

    private void RefreshAllInventoryViews()
    {
        if (normalPlayerInventoryUI != null)
            normalPlayerInventoryUI.Refresh();

        if (ovenPlayerInventoryUI != null)
            ovenPlayerInventoryUI.Refresh();

        if (ovenInventoryUI != null)
            ovenInventoryUI.Refresh();
    }

    private void OnBakePressed()
    {
        if (_isBaking) return;
        if (!HasRequiredIngredients()) return;

        StartCoroutine(BakeRoutine());
    }

    private bool HasRequiredIngredients()
    {
        if (recipe == null || recipe.requirements == null) return false;

        foreach (var requirement in recipe.requirements)
        {
            if (!ovenInventory.Has(requirement.item, requirement.amount))
                return false;
        }

        return true;
    }

    private IEnumerator BakeRoutine()
    {
        _isBaking = true;
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        foreach (var requirement in recipe.requirements)
            ovenInventory.Remove(requirement.item, requirement.amount);

        playerInventory.Add(recipe.result, recipe.resultAmount);

        if (audioSource != null && bakeCompleteSfx != null)
            audioSource.PlayOneShot(bakeCompleteSfx);

        RefreshAllInventoryViews();

        yield return new WaitForSecondsRealtime(0.2f);

        _isBaking = false;
        UpdateHighlights();
    }

    private void UpdateHighlights()
    {
        ClearAllHighlights();

        if (_isOpen)
        {
            if (_selectingPlayerSide)
            {
                if (ovenPlayerInventoryUI != null)
                    ovenPlayerInventoryUI.SetHighlight(_playerIndex, true);
            }
            else
            {
                if (ovenInventoryUI != null)
                    ovenInventoryUI.SetHighlight(_ovenIndex, true);
            }
        }
        else
        {
            if (normalPlayerInventoryUI != null)
                normalPlayerInventoryUI.SetHighlight(_playerIndex, true);
        }
    }

    private void ClearAllHighlights()
    {
        if (normalPlayerInventoryUI != null)
        {
            for (int i = 0; i < normalPlayerInventoryUI.SlotCount; i++)
                normalPlayerInventoryUI.SetHighlight(i, false);
        }

        if (ovenPlayerInventoryUI != null)
        {
            for (int i = 0; i < ovenPlayerInventoryUI.SlotCount; i++)
                ovenPlayerInventoryUI.SetHighlight(i, false);
        }

        if (ovenInventoryUI != null)
        {
            for (int i = 0; i < ovenInventoryUI.SlotCount; i++)
                ovenInventoryUI.SetHighlight(i, false);
        }
    }
}