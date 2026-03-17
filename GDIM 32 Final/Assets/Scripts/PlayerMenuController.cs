using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private InventoryUI playerInventoryUI;
    public GameObject pressTabPanel;

    [Header("Controls")]
    [SerializeField] private int gridColumns = 4;
    private KeyCode escKey = KeyCode.Escape;
    private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private KeyCode deleteKey = KeyCode.O;
    public bool _inventoryDisabled;
    private bool _isOpen;
    private int _selectedIndex;

    public bool IsOpen => _isOpen;

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (playerInventoryUI != null)
            playerInventoryUI.SetInventory(playerInventory);

        ClearHighlights();
    }

    private void Update()
    {

        if (!_inventoryDisabled)
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (!_isOpen)
                    OpenMenu();
                else
                    CloseMenu();

                return;
            }  
            if (Input.GetKeyDown(escKey))
            {
                if (_isOpen)
                    CloseMenu();

                return;
            }  

            if (!_isOpen)
                return;

            HandleNavigation();

            if (Input.GetKeyDown(deleteKey))
                DeleteSelectedItem();            
                    
        }



    }

    private void OpenMenu()
    {
        _isOpen = true;
        _selectedIndex = Mathf.Clamp(_selectedIndex, 0, Mathf.Max(0, playerInventoryUI.SlotCount - 1));

        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);

        if (pressTabPanel != null)
            pressTabPanel.SetActive(false);

        Time.timeScale = 0f;

        if (playerInventoryUI != null)
            playerInventoryUI.Refresh();

        UpdateHighlight();
    }

    private void CloseMenu()
    {
        _isOpen = false;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (pressTabPanel != null)
            pressTabPanel.SetActive(true);

        ClearHighlights();
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
        int max = playerInventoryUI != null ? playerInventoryUI.SlotCount : 0;
        int newIndex = _selectedIndex + direction * gridColumns;

        if (newIndex >= 0 && newIndex < max)
            _selectedIndex = newIndex;

        UpdateHighlight();
    }

    private void MoveHorizontal(int direction)
    {
        int max = playerInventoryUI != null ? playerInventoryUI.SlotCount : 0;
        int col = _selectedIndex % gridColumns;

        if (direction < 0)
        {
            if (col > 0)
                _selectedIndex--;
        }
        else
        {
            if (col < gridColumns - 1 && _selectedIndex + 1 < max)
                _selectedIndex++;
        }

        UpdateHighlight();
    }

    private void DeleteSelectedItem()
    {
        if (playerInventory == null) return;

        InventorySlot slot = playerInventory.GetSlot(_selectedIndex);
        if (slot == null || slot.IsEmpty) return;

        playerInventory.RemoveFromSlot(_selectedIndex, 1);

        if (playerInventoryUI != null)
            playerInventoryUI.Refresh();

        int maxIndex = Mathf.Max(0, playerInventoryUI.SlotCount - 1);
        _selectedIndex = Mathf.Clamp(_selectedIndex, 0, maxIndex);

        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        ClearHighlights();

        if (_isOpen && playerInventoryUI != null)
            playerInventoryUI.SetHighlight(_selectedIndex, true);
    }

    private void ClearHighlights()
    {
        if (playerInventoryUI == null) return;

        for (int i = 0; i < playerInventoryUI.SlotCount; i++)
            playerInventoryUI.SetHighlight(i, false);
    }
}
