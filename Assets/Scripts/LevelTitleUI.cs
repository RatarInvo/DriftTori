using System.Collections;
using UnityEngine;
using TMPro;

public class LevelTitleUI : MonoBehaviour
{
    public static LevelTitleUI Instance;

    [Header("Fade Settings")]
    public float fadeInDuration = 1f;
    public float holdDuration = 5f;
    public float fadeOutDuration = 1f;

    TextMeshProUGUI titleText;
    Coroutine currentSequence;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        titleText = GetComponent<TextMeshProUGUI>();
        SetAlpha(0f);
    }

    // Called by LevelManager — level index is zero-based so +1 for display
    public void ShowTitle(int levelIndex)
    {
        titleText.text = $"Level {levelIndex + 1}";

        if (currentSequence != null) StopCoroutine(currentSequence);
        currentSequence = StartCoroutine(TitleSequence());
    }

    // Called by carInput when W is pressed — skips the hold and fades out immediately
    public void HideTitle()
    {
        if (currentSequence != null) StopCoroutine(currentSequence);
        currentSequence = StartCoroutine(FadeOut());
    }

    IEnumerator TitleSequence()
    {
        // Fade in
        yield return StartCoroutine(FadeIn());

        // Hold for holdDuration seconds
        yield return new WaitForSeconds(holdDuration);

        // Fade out automatically after hold
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float startAlpha = titleText.color.a;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, 1f, elapsed / fadeInDuration));
            yield return null;
        }
        SetAlpha(1f);
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float startAlpha = titleText.color.a;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, 0f, elapsed / fadeOutDuration));
            yield return null;
        }
        SetAlpha(0f);
    }

    void SetAlpha(float alpha)
    {
        Color c = titleText.color;
        c.a = alpha;
        titleText.color = c;
    }
}