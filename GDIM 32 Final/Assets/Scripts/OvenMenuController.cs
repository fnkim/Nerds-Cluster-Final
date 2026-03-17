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
    [SerializeField] private GameObject bakeButtonHighlight;

    [Header("Recipe")]
    [SerializeField] private RecipeData recipe;

    [Header("Grid Settings")]
    [SerializeField] private int playerGridColumns = 4;
    [SerializeField] private int ovenGridColumns = 3;
    [SerializeField] private PlayerMenuController _playerMenuController;


    [SerializeField] private SetQuestState _setQuestState;

    private bool _isOpen;
    private int _playerIndex;
    private int _ovenIndex;

    private enum OvenSelectionArea
    {
        PlayerInventory,
        OvenInventory,
        BakeButton
    }

    private OvenSelectionArea _currentArea = OvenSelectionArea.PlayerInventory;

    public bool IsOpen => _isOpen;

    private void Start()
    {
        if (ovenMenuRoot != null)
            ovenMenuRoot.SetActive(false);

        if (normalPlayerInventoryUI != null)
            normalPlayerInventoryUI.SetInventory(playerInventory);

        if (ovenPlayerInventoryUI != null)
            ovenPlayerInventoryUI.SetInventory(playerInventory);

        if (ovenInventoryUI != null)
            ovenInventoryUI.SetInventory(ovenInventory);

        ClearAllHighlights();

        if (bakeButton != null)
            bakeButton.onClick.AddListener(Bake);
    }

    private void Update()
    {
        if (!_isOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
            return;
        }

        HandleNavigation();

        if (Input.GetKeyDown(KeyCode.T))
            HandleTransferOrBake();
    }

    public void OpenMenu()
    {
        if (_isOpen) return;
        _playerMenuController._inventoryDisabled = true;
        _playerMenuController.pressTabPanel.SetActive(false);
        _isOpen = true;
        _playerIndex = 0;
        _ovenIndex = 0;
        _currentArea = OvenSelectionArea.PlayerInventory;

        if (ovenMenuRoot != null)
            ovenMenuRoot.SetActive(true);

        RefreshAllInventoryViews();
        Time.timeScale = 0f;
        UpdateHighlights();
    }

    public void CloseMenu()
    {
        _playerMenuController.pressTabPanel.SetActive(true);
        _playerMenuController._inventoryDisabled = false;
        if (!_isOpen) return;

        _isOpen = false;

        if (ovenMenuRoot != null)
            ovenMenuRoot.SetActive(false);

        ClearAllHighlights();
        Time.timeScale = 1f;
    }

    private void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveVertical(-1);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveVertical(1);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveHorizontal(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveHorizontal(1);
    }

    private void MoveVertical(int direction)
    {
        if (_currentArea == OvenSelectionArea.PlayerInventory)
        {
            int max = ovenPlayerInventoryUI != null ? ovenPlayerInventoryUI.SlotCount : 0;
            int newIndex = _playerIndex + direction * playerGridColumns;

            if (newIndex >= 0 && newIndex < max)
                _playerIndex = newIndex;
        }
        else if (_currentArea == OvenSelectionArea.OvenInventory)
        {
            int max = ovenInventoryUI != null ? ovenInventoryUI.SlotCount : 0;
            int newIndex = _ovenIndex + direction * ovenGridColumns;

            if (newIndex >= 0 && newIndex < max)
                _ovenIndex = newIndex;
            else if (direction > 0)
                _currentArea = OvenSelectionArea.BakeButton;
        }
        else if (_currentArea == OvenSelectionArea.BakeButton)
        {
            if (direction < 0)
                _currentArea = OvenSelectionArea.OvenInventory;
        }

        UpdateHighlights();
    }

    private void MoveHorizontal(int direction)
    {
        if (_currentArea == OvenSelectionArea.PlayerInventory)
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
                    _currentArea = OvenSelectionArea.OvenInventory;
            }
        }
        else if (_currentArea == OvenSelectionArea.OvenInventory)
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
                    _currentArea = OvenSelectionArea.PlayerInventory;
            }
        }

        UpdateHighlights();
    }

    private void HandleTransferOrBake()
    {
        if (_currentArea == OvenSelectionArea.PlayerInventory)
            TransferToOven();
        else if (_currentArea == OvenSelectionArea.OvenInventory)
            TransferToPlayer();
        else if (_currentArea == OvenSelectionArea.BakeButton)
            Bake();
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

    public void Bake()
    {
        if (!HasRequiredIngredients())
        {
            Debug.Log("Missing recipe ingredients.");
            return;
        }

        foreach (var requirement in recipe.requirements)
        {
            ovenInventory.Remove(requirement.item, requirement.amount);
        }

        playerInventory.Add(recipe.result, recipe.resultAmount);

        _setQuestState._questToSet.QuestState = _setQuestState._questStateToSet;

        RefreshAllInventoryViews();
        UpdateHighlights();

        Debug.Log("Candy Tart baked!");
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

    private void RefreshAllInventoryViews()
    {
        if (normalPlayerInventoryUI != null)
            normalPlayerInventoryUI.Refresh();

        if (ovenPlayerInventoryUI != null)
            ovenPlayerInventoryUI.Refresh();

        if (ovenInventoryUI != null)
            ovenInventoryUI.Refresh();
    }

    private void UpdateHighlights()
    {
        ClearAllHighlights();

        if (_currentArea == OvenSelectionArea.PlayerInventory && ovenPlayerInventoryUI != null)
        {
            ovenPlayerInventoryUI.SetHighlight(_playerIndex, true);
        }
        else if (_currentArea == OvenSelectionArea.OvenInventory && ovenInventoryUI != null)
        {
            ovenInventoryUI.SetHighlight(_ovenIndex, true);
        }
        else if (_currentArea == OvenSelectionArea.BakeButton && bakeButtonHighlight != null)
        {
            bakeButtonHighlight.SetActive(true);
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

        if (bakeButtonHighlight != null)
            bakeButtonHighlight.SetActive(false);
    }
}