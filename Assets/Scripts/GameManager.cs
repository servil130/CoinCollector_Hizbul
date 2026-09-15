using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int coins;
    public PlayerMovement player;
    private int collected = 0;

    public TMP_Text coinsText;
    public GameObject winPanel;
    private int hitung = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPanel.SetActive(false);
        UpdetTeks();
        collected = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void CollectCoin()
    {
        collected=player.score;
        UpdetTeks();
        if (collected >= 5)
        {
            Win();
        }
        else
        {
            Debug.Log("Collected: " + collected);
        }
    }
    public void UpdetTeks()
    {
        coinsText.text = "Coins: " + collected.ToString();
    }
    void Win()
    {
        winPanel.SetActive(true);
        Debug.Log("Anjay menang");
        coinsText.text = "Win coy";
    }
}
