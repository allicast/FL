using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject MainMenuCanvas;
    public GameObject settingsPanel;
    public GameObject blackScreenPanel;

    [Header("Audio")]
    public AudioSource musicManager;      // AudioSource del menú
    public AudioClip transitionSound;     // sonido/canción para el momento en negro
    public float transitionDuration = 3f; // segundos antes de cambiar de escena

    public void PlayGame()
    {
        StartCoroutine(PlayTransition());
    }

    private System.Collections.IEnumerator PlayTransition()
    {
        // 1. Ocultar todo el menú
        MainMenuCanvas.SetActive(false);

        // 2. Pantalla negra ON
        blackScreenPanel.SetActive(true);

        // 3. Detener música del menú
        if (musicManager != null)
            musicManager.Stop();

        // 4. Reproducir sonido de transición
        if (transitionSound != null && musicManager != null)
        {
            musicManager.clip = transitionSound;
            musicManager.Play();
        }

        // 5. Esperar X segundos
        yield return new WaitForSeconds(transitionDuration);

        // 6. Cargar la siguiente escena
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