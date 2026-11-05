using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
    public string itemDescription;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool isFull;   // true kalau stack sudah mencapai maksimum

    // Item Slot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private int maxStackSize = 99;

    // Selected Item
    [SerializeField] public GameObject selectedPanel;
    public bool isItemSelected;

    public InventoryManager inventoryManager;

    public bool IsEmpty => string.IsNullOrEmpty(itemName);
    public int MaxStackSize => maxStackSize;

    // Sisa tempat di slot ini (slot kosong = muat satu stack penuh)
    public int FreeSpace => IsEmpty ? maxStackSize : Mathf.Max(0, maxStackSize - itemQuantity);

    void Awake()
    {
        // Slot selalu mulai kosong, walau prefab menyimpan nilai lama
        Clear();
        Deselect();
    }

    void Start()
    {
        if (inventoryManager == null)
            inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public bool CanStackWith(string otherItemName)
    {
        return !IsEmpty && !isFull && itemName == otherItemName;
    }


    public int AddItem(ItemDetails item, int amount)
    {
        if (IsEmpty)
        {
            itemName = item.ItemName;
            itemDescription = item.Description;
            itemIcon = item.ItemIcon;
            itemQuantity = 0;
        }
        return AddQuantity(amount);
    }

    
    public int AddQuantity(int amount)
    {
        int added = Mathf.Clamp(amount, 0, maxStackSize - itemQuantity);
        itemQuantity += added;
        isFull = itemQuantity >= maxStackSize;
        RefreshUI();
        return amount - added;
    }

    public void RemoveQuantity(int amount)
    {
        itemQuantity -= amount;
        if (itemQuantity <= 0)
        {
            Clear();
            return;
        }
        isFull = itemQuantity >= maxStackSize;
        RefreshUI();
    }

    public void Clear()
    {
        itemName = "";
        itemDescription = "";
        itemQuantity = 0;
        itemIcon = null;
        isFull = false;
        RefreshUI();
    }

    public void Select()
    {
        isItemSelected = true;
        if (selectedPanel != null) selectedPanel.SetActive(true);
    }

    public void Deselect()
    {
        isItemSelected = false;
        if (selectedPanel != null) selectedPanel.SetActive(false);
    }

    private void RefreshUI()
    {
        if (quantityText != null)
        {
            quantityText.text = IsEmpty ? "" : itemQuantity.ToString();
            quantityText.enabled = !IsEmpty;
        }

        if (itemImage != null)
        {
            itemImage.sprite = itemIcon;
            itemImage.preserveAspect = true;
            itemImage.enabled = !IsEmpty && itemIcon != null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
    }

    // Klik kiri: pilih slot dan tampilkan detail item
    private void LeftClick()
    {
        if (inventoryManager == null)
            inventoryManager = FindAnyObjectByType<InventoryManager>();

        // Hapus pilihan sebelumnya (sekaligus menyembunyikan panel detail)
        if (inventoryManager != null) inventoryManager.DesselectAllSlot();

        if (IsEmpty) return;   // slot kosong tidak bisa dipilih

        Debug.Log("Left Clicked on " + itemName);
        Select();
        if (inventoryManager != null) inventoryManager.ShowDetails(this);
    }

    // Klik kanan: buang 1 item dari stack
    private void RightClick()
    {
        if (IsEmpty) return;

        Debug.Log("Right Clicked on " + itemName + " (buang 1)");
        RemoveQuantity(1);
        if (inventoryManager != null) inventoryManager.OnSlotChanged(this);
    }
}