using UnityEngine;

public class finishLine : MonoBehaviour
{
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
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            triggered = false;
    }
}