using UnityEngine;
using UnityEngine.InputSystem;

public class DebugTools : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Debug Coins")]
    public int coinAmount = 10;

    void Update()
    {
        // Press F1 to add coins instantly
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            for (int i = 0; i < coinAmount; i++)
                CoinManager.Instance.AddCoin();

            Debug.Log($"[Debug] Added {coinAmount} coins. Total: {CoinManager.Instance.GetCoins()}");
        }

        // Press F2 to reset coins and upgrades
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            CoinManager.Instance.ResetCoins();
            UpgradeManager.Instance.ResetUpgrades();
            Debug.Log("[Debug] Coins and upgrades reset.");
        }
    }
#endif
}