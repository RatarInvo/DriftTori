using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("References")]
    public CarController car;
    public LivesSystem livesSystem;
    public Light2D headlight;

    [Header("Upgrade Costs")]
    public int driftUpgradeCost = 3;
    public int heartsUpgradeCost = 5;
    public int headlightUpgradeCost = 4;

    [Header("Upgrade Limits")]
    public int maxDriftUpgrades = 3;
    public int maxHeartsUpgrades = 2;
    public int maxHeadlightUpgrades = 1;

    public int DriftLevel { get; private set; }
    public int HeartsLevel { get; private set; }
    public int HeadlightLevel { get; private set; }

    const float driftTurnBonus = 0.4f;
    const float baseTurnFactor = 3.75f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadUpgrades();
        ApplyAllUpgrades();
    }

    void LoadUpgrades()
    {
        DriftLevel = PlayerPrefs.GetInt("UpgradeDrift", 0);
        HeartsLevel = PlayerPrefs.GetInt("UpgradeHearts", 0);
        HeadlightLevel = PlayerPrefs.GetInt("UpgradeHeadlight", 0);
    }

    void SaveUpgrades()
    {
        PlayerPrefs.SetInt("UpgradeDrift", DriftLevel);
        PlayerPrefs.SetInt("UpgradeHearts", HeartsLevel);
        PlayerPrefs.SetInt("UpgradeHeadlight", HeadlightLevel);
        PlayerPrefs.Save();
    }

    public void ApplyAllUpgrades()
    {
        if (!levelManager.campaignMode)
            return;

        ApplyDrift();
        ApplyHearts();
        ApplyHeadlight();
    }

    // DRIFT
    public bool CanUpgradeDrift() =>
        DriftLevel < maxDriftUpgrades &&
        CoinManager.Instance.GetCoins() >= driftUpgradeCost;

    public void UpgradeDrift()
    {
        if (!CanUpgradeDrift()) return;
        CoinManager.Instance.SpendCoins(driftUpgradeCost);
        DriftLevel++;
        SaveUpgrades();
        ApplyDrift();
    }

    void ApplyDrift()
    {
        if (car == null) return;
        car.turnFactor = baseTurnFactor + DriftLevel * driftTurnBonus;
    }

    // HEARTS
    public bool CanUpgradeHearts() =>
        HeartsLevel < maxHeartsUpgrades &&
        CoinManager.Instance.GetCoins() >= heartsUpgradeCost;

    public void UpgradeHearts()
    {
        if (!CanUpgradeHearts()) return;
        CoinManager.Instance.SpendCoins(heartsUpgradeCost);
        HeartsLevel++;
        SaveUpgrades();
        ApplyHearts();
    }

    void ApplyHearts()
    {
        if (livesSystem == null) return;
        livesSystem.maxLives = 3 + HeartsLevel;
        livesSystem.ResetLives();
    }

    // HEADLIGHT
    public bool CanUpgradeHeadlight() =>
        HeadlightLevel < maxHeadlightUpgrades &&
        CoinManager.Instance.GetCoins() >= headlightUpgradeCost;

    public void UpgradeHeadlight()
    {
        if (!CanUpgradeHeadlight()) return;
        CoinManager.Instance.SpendCoins(headlightUpgradeCost);
        HeadlightLevel++;
        SaveUpgrades();
        ApplyHeadlight();
    }

    void ApplyHeadlight()
    {
        if (headlight == null || HeadlightLevel < 1) return;
        Color c = headlight.color;
        c.a = 1f; // Full opacity 255
        headlight.color = c;
    }

    public void ResetUpgrades()
    {
        DriftLevel = 0;
        HeartsLevel = 0;
        HeadlightLevel = 0;
        SaveUpgrades();
        ApplyAllUpgrades();
    }
}