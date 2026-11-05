public interface GrabableItem
{
    // Mengembalikan true kalau item berhasil masuk seluruhnya.
    // Kalau false (misal inventory penuh), item di dunia tidak boleh dihapus.
    bool AddItem(ItemDetails itemDetails);
}