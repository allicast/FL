using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuCanvas;
    public GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Cinematica");
    }

    public void OpenSettings()
    {
        MainMenuCanvas.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
