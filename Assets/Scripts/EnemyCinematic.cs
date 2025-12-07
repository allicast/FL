using UnityEngine;

public class EnemyCinematic : MonoBehaviour
{
    [Header("Cinemática")]
    public Transform cinematicPoint;
    public float cameraMoveSpeed = 3f;
    public float cinematicDuration = 2f;
    public bool lockPlayerControl = true;

    [Header("Cámaras")]
    public Camera playerCamera;
    public Camera cinematicCamera;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public AudioSource gameOverSound;

    [Header("Jugador")]
    public GameObject playerModel;

    private bool cinematicActive = false;
    private float cinematicTimer = 0f;

    private void Start()
    {
        if (cinematicCamera == null)
            cinematicCamera = Camera.main;

        if (cinematicPoint == null)
            Debug.LogWarning("EnemyCinematic: No se asignó el CinematicPoint en el inspector.");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (cinematicCamera != null)
            cinematicCamera.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCinematic(other.gameObject);
        }
    }

    void StartCinematic(GameObject player)
    {
        if (cinematicActive) return;

        cinematicActive = true;
        cinematicTimer = 0f;

        // 1. Desactivar cámara del jugador
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);

        // 2. Activar cámara cinemática
        if (cinematicCamera != null)
            cinematicCamera.gameObject.SetActive(true);

        // 4. Ocultar modelo jugador
        if (playerModel != null)
            playerModel.SetActive(false);

        // 5. Desactivar control jugador
        if (lockPlayerControl)
        {
            var controller = player.GetComponent<MonoBehaviour>();
            if (controller != null)
                controller.enabled = false;
        }

        // 6. Detener IA enemigo
        var enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null && enemyAI.agent != null)
            enemyAI.agent.ResetPath();
    }

    private void LateUpdate()
    {
        if (!cinematicActive || cinematicCamera == null)
            return;

        cinematicTimer += Time.deltaTime;

        cinematicCamera.transform.position = Vector3.Lerp(
            cinematicCamera.transform.position,
            cinematicPoint.position,
            Time.deltaTime * cameraMoveSpeed
        );

        // Hacer que mire al enemigo
        cinematicCamera.transform.LookAt(transform.position + Vector3.up * 1.0f);

        // Finalizar cinemática
        if (cinematicTimer >= cinematicDuration)
        {
            EndCinematic();
        }
    }

    void EndCinematic()
    {
        cinematicActive = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        AudioListener.volume = 0f;

        if (gameOverSound != null)
        {
            gameOverSound.ignoreListenerVolume = true;
            gameOverSound.Play();
        }
    }
}

