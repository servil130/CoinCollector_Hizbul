using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails", menuName = "Scriptable Objects/ItemDetails")]
public class ItemDetails : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField, TextArea(2, 5)] private string description;
    [SerializeField] private int quantity = 1;
    [SerializeField] private Sprite itemIcon;

    public string ItemName => itemName;
    public string Description => description;
    public int Quantity => quantity;
    public Sprite ItemIcon => itemIcon;
}