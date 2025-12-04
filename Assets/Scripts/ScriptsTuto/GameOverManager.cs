using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Respawn")]
    public Transform respawnPoint;     // Punto donde aparecerá el player al reintentar
    public GameObject player;          // Referencia al jugador

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;   // El panel que se activa

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Ocultar al inicio
    }

    // Este método lo llama tu EnemyCinematic
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // ------- BOTONES ---------

    // REINTENTAR → respawnear al jugador
    public void RetryGame()
    {
        if (player != null && respawnPoint != null)
        {
            // Mover al jugador al punto indicado
            player.transform.position = respawnPoint.position;

            // Reactivar controles si lo necesitas
            var controller = player.GetComponent<MonoBehaviour>();
            if (controller != null)
                controller.enabled = true;
        }

        // Restaurar audio
        AudioListener.volume = 1f;

        // Ocultar panel
        gameOverPanel.SetActive(false);
    }

    // SALIR → ir a escena "UI"
    public void ExitToMenu()
    {
        SceneManager.LoadScene("UI");
    }
}