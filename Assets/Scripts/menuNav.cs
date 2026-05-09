using UnityEngine;
using UnityEngine.SceneManagement;

public class menuNav : MonoBehaviour
{
    public GameObject guideText;

    public void PlayGame()
    {
        levelManager.campaignMode = false;
        SceneManager.LoadScene("Game");
    }

    public void PlayCampaign()
    {
        levelManager.campaignMode = true;
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