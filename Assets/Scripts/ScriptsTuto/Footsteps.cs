using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public float walkPitch = 1f;
    public float runPitch = 1.4f;

    private PlayerMove player;

    void Start()
    {
        player = GetComponent<PlayerMove>();
    }

    void Update()
    {
        // Si está en inventario o interactuando, NO debe sonar
        if (PlayerMove.isInventoryOpen)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }

        // Si no hay animación de movimiento, silencio
        bool isMoving = player != null && playerAnimMoving();

        if (!isMoving)
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
            return;
        }

        // Si se está moviendo, reproducir sonido
        if (!audioSource.isPlaying)
            audioSource.Play();

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        audioSource.pitch = isRunning ? runPitch : walkPitch;
    }

    bool playerAnimMoving()
    {
        // Usa exactamente las variables que tu animación usa
        return player.newDirection.magnitude > 0.1f;
    }
}
