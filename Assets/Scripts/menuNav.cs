using UnityEngine;
using UnityEngine.SceneManagement;

public class menuNav : MonoBehaviour
{
    public GameObject guideText;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadGuide()
    {
        guideText.SetActive(!guideText.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}