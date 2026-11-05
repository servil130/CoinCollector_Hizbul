using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private int slotCount = 16;
    [SerializeField] private ItemSlot slotPrefab;     
    [SerializeField] private Transform slotParent;   
    [SerializeField] private ItemDetailsPanel detailsPanel;   
    private bool InventoryActive;
    public ItemSlot[] itemSlot;

    void Start()
    {
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        if (inventoryAction == null)
        {
            Debug.LogError("Action 'Inventory' tidak ditemukan di Input Actions.", this);
        }

        CreateSlots();

        if (InventoryCanvas != null) InventoryCanvas.SetActive(false);
        InventoryActive = false;
        DesselectAllSlot();
    }

    void Update()
    {
        if (inventoryAction == null || InventoryCanvas == null) return;

        if (inventoryAction.triggered)
        {
            InventoryActive = !InventoryActive;
            Time.timeScale = InventoryActive ? 0f : 1f;
            InventoryCanvas.SetActive(InventoryActive);
            Debug.Log(InventoryActive ? "Inventory is active" : "Inventory is deactivated");

            DesselectAllSlot();
        }
    }

    // Buat slot saat runtime
    private void CreateSlots()
    {
        ItemSlot prefab = slotPrefab != null ? slotPrefab : Resources.Load<ItemSlot>("Slot");
        if (prefab == null)
        {
            Debug.LogError("Slot Prefab kosong dan 'Slot' tidak ada di folder Resources.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        Transform parent = slotParent;
        if (parent == null)
        {
            GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
            parent = grid != null ? grid.transform : InventoryCanvas.transform;
            Debug.LogWarning("Slot Parent kosong, memakai: " + parent.name, this);
        }

        itemSlot = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(prefab, parent);
            itemSlot[i].inventoryManager = this;
        }
    }


    public bool AddItem(ItemDetails item)
    {
        if (item == null) return false;

        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

        if (itemSlot == null || itemSlot.Length == 0)
        {
            Debug.LogWarning("Inventory has no slots, '" + item.ItemName + "' was not added.");
            return false;
        }

        // Cek dulu apakah semuanya muat
        int freeSpace = 0;
        foreach (ItemSlot slot in itemSlot)
        {
            if (slot.IsEmpty || slot.CanStackWith(item.ItemName))
                freeSpace += slot.FreeSpace;
        }

        if (freeSpace < item.Quantity)
        {
            Debug.LogWarning("Inventory is full, '" + item.ItemName + "' was not added.");
            return false;
        }

        int remaining = item.Quantity;

        for (int i = 0; i < itemSlot.Length && remaining > 0; i++)
        {
            if (itemSlot[i].CanStackWith(item.ItemName))
            {
                remaining = itemSlot[i].AddQuantity(remaining);
            }
        }

        for (int i = 0; i < itemSlot.Length && remaining > 0; i++)
        {
            if (itemSlot[i].IsEmpty)
            {
                remaining = itemSlot[i].AddItem(item, remaining);
            }
        }

        RefreshSelectedDetails();
        return true;
    }

    
    public void ShowDetails(ItemSlot slot)
    {
        detailsPanel.Show(slot);
    }


    public void OnSlotChanged(ItemSlot slot)
    {
        if (!slot.isItemSelected) return;

        if (slot.IsEmpty)
        {
            slot.Deselect();
            if (detailsPanel != null) detailsPanel.Hide();
        }
        else if (detailsPanel != null)
        {
            detailsPanel.Show(slot);
        }
    }

  
    private void RefreshSelectedDetails()
    {
        foreach (ItemSlot slot in itemSlot)
        {
            if (slot.isItemSelected) OnSlotChanged(slot);
        }
    }

    public void DesselectAllSlot()
    {
        
            foreach (ItemSlot slot in itemSlot)
            {
                slot.Deselect();
            }
        

        if (detailsPanel != null) detailsPanel.Hide();
    }
}