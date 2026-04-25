using UnityEngine;

public class finishLine : MonoBehaviour
{
    // Optional: prevent double-triggering if car lingers
    bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            levelManager.Instance.AdvanceToNextLevel();
        }
    }

    // Reset when car leaves (in case LevelManager loops or replays)
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            triggered = false;
    }
}