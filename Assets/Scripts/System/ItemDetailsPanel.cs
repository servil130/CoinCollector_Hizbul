using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemQuantityText;

    // Tampilkan detail dari slot yang dipilih
    public void Show(ItemSlot slot)
    {
        if (slot == null || slot.IsEmpty)
        {
            Hide();
            return;
        }

        gameObject.SetActive(true);

        if (itemNameText != null)
            itemNameText.text = slot.itemName;

        if (itemDescriptionText != null)
            itemDescriptionText.text = string.IsNullOrEmpty(slot.itemDescription)
                ? "Tidak ada deskripsi."
                : slot.itemDescription;

        if (itemQuantityText != null)
            itemQuantityText.text = slot.itemQuantity.ToString();

        if (itemIconImage != null)
        {
            itemIconImage.sprite = slot.itemIcon;
            itemIconImage.preserveAspect = true;
            itemIconImage.enabled = slot.itemIcon != null;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}