using UnityEngine;

public class EnemyCinematic : MonoBehaviour
{
    [Header("Cinemática")]
    public Transform cinematicPoint;
    public float cameraMoveSpeed = 3f;
    public float cinematicDuration = 2f;
    public bool lockPlayerControl = true;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public AudioSource gameOverSound;

    [Header("Jugador")]
    public GameObject playerModel;   // 👈 AÑADIDO: el modelo 3D de tu jugador

    private Camera mainCam;
    private bool cinematicActive = false;
    private float cinematicTimer = 0f;

    private void Start()
    {
        mainCam = Camera.main;

        if (cinematicPoint == null)
            Debug.LogWarning("EnemyCinematic: No se asignó el CinematicPoint en el inspector.");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
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

        // 👈 AÑADIDO: ocultar el modelo 3D del jugador
        if (playerModel != null)
            playerModel.SetActive(false);

        // Desactivar control del jugador
        if (lockPlayerControl)
        {
            var controller = player.GetComponent<MonoBehaviour>();
            if (controller != null)
                controller.enabled = false;
        }

        // Detener IA del enemigo si existe
        var enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null && enemyAI.agent != null)
        {
            enemyAI.agent.ResetPath();
        }
    }

    private void LateUpdate()
    {
        if (!cinematicActive || mainCam == null || cinematicPoint == null)
            return;

        cinematicTimer += Time.deltaTime;

        mainCam.transform.position = Vector3.Lerp(
            mainCam.transform.position,
            cinematicPoint.position,
            Time.deltaTime * cameraMoveSpeed
        );

        mainCam.transform.LookAt(transform.position + Vector3.up * 1.0f);

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

        AudioListener.volume = 0f;

        if (gameOverSound != null)
        {
            gameOverSound.ignoreListenerVolume = true;
            gameOverSound.Play();
        }

        // No es necesario volver a activar el jugador porque es Game Over
    }
}
