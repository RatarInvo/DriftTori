using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip pickupSound;

    [Header("Visuals")]
    public float rotateSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance.PlaySFX(pickupSound);
        CoinManager.Instance.AddCoin();
        Destroy(gameObject);
    }
}