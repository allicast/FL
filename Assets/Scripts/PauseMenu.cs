using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject crosshair;
    private bool isPaused = false;

    public GameObject settingsPanel;
    public GameObject controlsPanel;
    public GameObject soundPanel;

    public ThirdPersonController playerController;
    public List<MonoBehaviour> scriptsToFreeze = new List<MonoBehaviour>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);

            // Congelar jugador
            if (playerController != null)
                playerController.enabled = false;

            // Congelar otros scripts
            foreach (var script in scriptsToFreeze)
                if (script != null) script.enabled = false;

            settingsPanel.SetActive(false);
            controlsPanel.SetActive(false);
            soundPanel.SetActive(false);

            // Pausar TODOS los sonidos
            AudioListener.pause = true;

            // Activar cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (crosshair != null)
                crosshair.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);

            // Activar jugador
            if (playerController != null)
                playerController.enabled = true;

            // Reactivar scripts
            foreach (var script in scriptsToFreeze)
                if (script != null) script.enabled = true;

            // Reanudar TODOS los sonidos
            AudioListener.pause = false;

            // Ocultar cursor (lo que querías)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (crosshair != null)
                crosshair.SetActive(true);

            pausePanel.SetActive(false);
            settingsPanel.SetActive(false);
            controlsPanel.SetActive(false);
            soundPanel.SetActive(false);
        }
    }

    public void OpenControls()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        controlsPanel.SetActive(true);
        soundPanel.SetActive(false);

        // Cursor visible para este panel
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenSound()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        soundPanel.SetActive(true);
        controlsPanel.SetActive(false);

        // Cursor visible para este panel
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToPause()
    {
        pausePanel.SetActive(true);

        settingsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        soundPanel.SetActive(false);

        // Cursor visible en el menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToSettings()
    {
        controlsPanel.SetActive(false);
        soundPanel.SetActive(false);

        settingsPanel.SetActive(true);

        // Cursor visible en settings
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (isPaused)
            TogglePause();
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("UI");
    }
}



