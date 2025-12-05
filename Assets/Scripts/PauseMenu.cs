using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject crosshair;
    private bool isPaused = false;

    // Aquí guardamos una referencia al script de la cámara
    public ThirdPersonController playerController;

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
            // Activar pausa
            Time.timeScale = 0f;
            pausePanel.SetActive(true);

            // Desactivar el movimiento de la cámara y jugador
            if (playerController != null)
            {
                playerController.enabled = false; // Desactivar el script del controlador del jugador
            }

            // Activar cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Ocultar mira
            if (crosshair != null)
                crosshair.SetActive(false);
        }
        else
        {
            // Quitar pausa
            Time.timeScale = 1f;
            pausePanel.SetActive(false);

            // Activar el movimiento de la cámara y jugador
            if (playerController != null)
            {
                playerController.enabled = true; // Reactivar el script del controlador del jugador
            }

            // Bloquear cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Mostrar mira
            if (crosshair != null)
                crosshair.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            TogglePause();
        }
    }

    // 🚪 NUEVA FUNCIÓN: SALIR A LA ESCENA "UI"
    public void ExitGame()
    {
        Time.timeScale = 1f; // Asegura que la escena nueva no quede pausada
        SceneManager.LoadScene("UI");
    }
}

