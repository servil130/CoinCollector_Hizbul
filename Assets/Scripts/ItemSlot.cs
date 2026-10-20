using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
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

    void Awake()
    {
        RefreshUI();
    }

    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public void AddItem(ItemDetails item)
    {
        itemName = item.ItemName;
        itemIcon = item.ItemIcon;
        itemQuantity = 0;
        AddQuantity(item.Quantity);
    }

    // Slot berisi item yang sama dan masih muat
    public bool CanStackWith(string otherItemName)
    {
        return !IsEmpty && !isFull && itemName == otherItemName;
    }

    public void AddQuantity(int amount)
    {
        itemQuantity += amount;
        isFull = itemQuantity >= maxStackSize;
        RefreshUI();
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

    private void LeftClick()
    {
        Debug.Log("Left Clicked on " + itemName);
        if (inventoryManager != null) inventoryManager.DesselectAllSlot();
        if (selectedPanel != null) selectedPanel.SetActive(true);
        isItemSelected = true;
    }

    private void RightClick()
    {
        Debug.Log("Right Clicked on " + itemName);
    }
}