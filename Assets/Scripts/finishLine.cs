using UnityEngine;
using System.Collections;

public class finishLine : MonoBehaviour
{
    [Tooltip("Seconds to wait after crossing before teleporting")]
    public float transitionDelay = 2f;

    bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) 
            return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            CarController car = other.GetComponent<CarController>();
            StartCoroutine(FinishSequence(car));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            triggered = false;
    }

    IEnumerator FinishSequence(CarController car)
    {
        car.isFinishing = true;
        car.BrakeToStop();
        yield return new WaitForSeconds(transitionDelay);
        levelManager.Instance.AdvanceToNextLevel();
    }
}