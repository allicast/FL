using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject crosshair;
    private bool isPaused = false;

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

            // Bloquear cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (crosshair != null)
                crosshair.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
            TogglePause();
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false; // por seguridad
        SceneManager.LoadScene("UI");
    }
}


