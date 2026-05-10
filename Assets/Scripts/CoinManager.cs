using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("UI")]
    public TextMeshProUGUI coinText;

    int totalCoins;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin()
    {
        totalCoins++;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = $"Coins: {totalCoins}";
    }
}