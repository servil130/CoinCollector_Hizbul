using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private int slotCount = 16;
    [SerializeField] private ItemSlot slotPrefab;     // opsional, kalau kosong pakai Resources/ItemSlot
    [SerializeField] private Transform slotParent;    // opsional, kalau kosong cari Grid Layout Group
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

    // Build the slot objects at runtime so no manual wiring is needed in the scene
    private void CreateSlots()
    {
        ItemSlot prefab = slotPrefab != null ? slotPrefab : Resources.Load<ItemSlot>("ItemSlot");
        if (prefab == null)
        {
            Debug.LogError("ItemSlot prefab was not found. Isi Slot Prefab atau taruh di folder Resources.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        Transform parent = slotParent;
        if (parent == null)
        {
            GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
            parent = grid != null ? grid.transform : InventoryCanvas.transform;
        }

        itemSlot = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(prefab, parent);
        }
    }

    public void AddItem(ItemDetails item)
    {
        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

        if (itemSlot == null || itemSlot.Length == 0)
        {
            Debug.LogWarning("Inventory has no slots, '" + item.ItemName + "' was lost.");
            return;
        }

        // 1. Tambah ke slot yang sudah berisi item sama
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].CanStackWith(item.ItemName))
            {
                itemSlot[i].AddQuantity(item.Quantity);
                return;
            }
        }

        // 2. Kalau belum ada, pakai slot yang benar-benar kosong
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].IsEmpty)
            {
                itemSlot[i].AddItem(item);
                return;
            }
        }

        Debug.LogWarning("Inventory is full, '" + item.ItemName + "' was lost.");
    }

    public void DesselectAllSlot()
    {
        if (itemSlot == null) return;

        foreach (ItemSlot slot in itemSlot)
        {
            slot.isItemSelected = false;
            if (slot.selectedPanel != null) slot.selectedPanel.SetActive(false);
        }
    }
}