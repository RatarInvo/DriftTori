using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("UI")]
    public TextMeshProUGUI coinText;
    public GameObject coinUIRoot;

    int totalCoins;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    void Start()
    {
        if (coinUIRoot != null)
            coinUIRoot.SetActive(levelManager.campaignMode);

        UpdateUI();
    }

    public void AddCoin()
    {
        totalCoins++;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        UpdateUI();
    }

    public void ResetCoins()
    {
        totalCoins = 0;
        PlayerPrefs.SetInt("TotalCoins", 0);
        PlayerPrefs.Save();
        UpdateUI();
    }

    public int GetCoins() => totalCoins;

    public bool SpendCoins(int amount)
    {
        if (totalCoins < amount) return false;
        totalCoins -= amount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = $"{totalCoins}";
    }
}