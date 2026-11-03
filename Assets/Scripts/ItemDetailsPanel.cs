using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemDetailsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemQuantityText;

    public void Show(ItemDetailsPanel item, int quantity)
    {
        
    }
}
