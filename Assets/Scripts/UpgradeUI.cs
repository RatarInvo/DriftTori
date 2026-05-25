using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance;

    [Header("Panel")]
    public GameObject upgradePanel;

    [Header("Coin Display")]
    public TextMeshProUGUI coinText;

    [Header("Drift Upgrade")]
    public Button driftButton;
    public TextMeshProUGUI driftLevelText;
    public TextMeshProUGUI driftCostText;

    [Header("Hearts Upgrade")]
    public Button heartsButton;
    public TextMeshProUGUI heartsLevelText;
    public TextMeshProUGUI heartsCostText;

    [Header("Headlight Upgrade")]
    public Button headlightButton;
    public TextMeshProUGUI headlightLevelText;
    public TextMeshProUGUI headlightCostText;

    [Header("Continue")]
    public Button continueButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        if (upgradePanel == null)
            upgradePanel = gameObject;

        upgradePanel.SetActive(false);
    }

    public void ShowUpgradePanel()
    {
        if (upgradePanel == null)
        {
            Debug.LogError("UpgradeUI: upgradePanel is not assigned or has been destroyed!");
            return;
        }

        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
        Refresh();
    }

    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Refresh()
    {
        UpgradeManager um = UpgradeManager.Instance;
        int coins = CoinManager.Instance.GetCoins();

        coinText.text = $"Coins: {coins}";

        // Drift
        bool driftMax = um.DriftLevel >= um.maxDriftUpgrades;
        driftLevelText.text = $"Level {um.DriftLevel}/{um.maxDriftUpgrades}";
        driftCostText.text = driftMax ? "MAX" : $"{um.driftUpgradeCost} coins";
        driftButton.interactable = um.CanUpgradeDrift();

        // Hearts
        bool heartsMax = um.HeartsLevel >= um.maxHeartsUpgrades;
        heartsLevelText.text = $"{3 + um.HeartsLevel}/5 hearts";
        heartsCostText.text = heartsMax ? "MAX" : $"{um.heartsUpgradeCost} coins";
        heartsButton.interactable = um.CanUpgradeHearts();

        // Headlight
        bool headlightMax = um.HeadlightLevel >= um.maxHeadlightUpgrades;
        headlightLevelText.text = headlightMax ? "Upgraded" : "Not upgraded";
        headlightCostText.text = headlightMax ? "MAX" : $"{um.headlightUpgradeCost} coins";
        headlightButton.interactable = um.CanUpgradeHeadlight();
    }

    public void OnClickDrift()
    {
        UpgradeManager.Instance.UpgradeDrift();
        Refresh();
    }

    public void OnClickHearts()
    {
        UpgradeManager.Instance.UpgradeHearts();
        Refresh();
    }

    public void OnClickHeadlight()
    {
        UpgradeManager.Instance.UpgradeHeadlight();
        Refresh();
    }

    public void OnClickContinue()
    {
        HideUpgradePanel();
    }
}