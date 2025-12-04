using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject MainMenuCanvas;
    public GameObject settingsPanel;
    public GameObject blackScreenPanel;

    [Header("Audio")]
    public AudioSource musicManager;           // Música del menú
    public AudioSource transitionSound;        // Sonido de transición
    public AudioSource[] sfxSources;           // <<< Agregado: todos los SFX del menú
    public float transitionDuration = 3f;

    public void PlayGame()
    {
        StartCoroutine(PlayTransition());
    }

    private System.Collections.IEnumerator PlayTransition()
    {
        // 1. Ocultar menú
        MainMenuCanvas.SetActive(false);

        // 2. Pantalla negra
        blackScreenPanel.SetActive(true);

        // 3. Detener música del menú
        if (musicManager != null)
            musicManager.Stop();

        // 4. Detener TODOS los SFX
        if (sfxSources != null)
        {
            foreach (AudioSource sfx in sfxSources)
            {
                if (sfx != null)
                    sfx.Stop();
            }
        }

        // 5. Reproducir sonido de transición
        if (transitionSound != null)
            transitionSound.Play();

        // 6. Esperar
        yield return new WaitForSeconds(transitionDuration);

        // 7. Cargar escena
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