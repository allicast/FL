using UnityEngine;

public class EnemyCinematic : MonoBehaviour
{
    [Header("Cinemática")]
    public Transform cinematicPoint;      // punto frente al enemigo
    public float cameraMoveSpeed = 3f;    // qué tan rápido se mueve la cámara
    public float cinematicDuration = 2f;  // cuánto dura la cinemática
    public bool lockPlayerControl = true; // si quieres desactivar el movimiento del player

    private Camera mainCam;
    private bool cinematicActive = false;
    private float cinematicTimer = 0f;

    private void Start()
    {
        mainCam = Camera.main;

        if (cinematicPoint == null)
        {
            Debug.LogWarning("EnemyCinematic: No se asignó el CinematicPoint en el inspector.");
        }
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

        // Opcional: desactivar control del jugador
        if (lockPlayerControl)
        {
            var controller = player.GetComponent<MonoBehaviour>();
            // Aquí deberías poner el script real de tu player, por ejemplo:
            // var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.enabled = false;
        }

        // También puedes parar al enemigo si quieres:
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

        // Mover la cámara hacia el punto frente al enemigo
        mainCam.transform.position = Vector3.Lerp(
            mainCam.transform.position,
            cinematicPoint.position,
            Time.deltaTime * cameraMoveSpeed
        );

        // Hacer que mire al enemigo
        mainCam.transform.LookAt(transform.position + Vector3.up * 1.0f);

        // Si ya pasó el tiempo de la cinemática, la apagamos
        if (cinematicTimer >= cinematicDuration)
        {
            cinematicActive = false;
            // Aquí podrías reactivar el control del player si lo desactivaste
        }
    }
}
