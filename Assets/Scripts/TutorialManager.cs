using Cinemachine;
using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [Header("UI y Referencias")]
    public TextMeshProUGUI tutorialText;
    public Transform npcLookTarget;
    public PlayerMove player;
    public CinemachineVirtualCamera playerCam, npcCam;

    [Header("Configuración")]
    public float camRotateSpeed = 3f;

    private bool tutorialActive = false;
    private Transform mainCam;
    public InputActionAsset defaultActions;

    void Start()
    {
        mainCam = Camera.main.transform;
        tutorialText.text = "";
    }

    public void PlayerReachedNPC()
    {
        if (!tutorialActive)
        {
            tutorialActive = true;
            StartCoroutine(SecuenciaTutorial());
        }
    }

    IEnumerator SecuenciaTutorial()
    {
        yield return StartCoroutine(EjecutarPaso(
            "¡Hola! Muévete hacia adelante con W o las flechas para comenzar.",
            0 // Paso 0: Se salta la verificación de movimiento
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Mira ese objeto y presiona 'E' para recogerlo.",
            1
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Ahora limpia la mesa. Acércate y presiona 'E' de nuevo.",
            2
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Puedes ver tus cosas abriendo el inventario con TAB.",
            3
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Por último, puedes pausar el juego con ESC.",
            4
        ));

        tutorialText.text = "¡Tutorial Completado! Buena suerte.";
        yield return new WaitForSeconds(3f);
        tutorialText.text = "";
        tutorialActive = false;
    }

    IEnumerator EjecutarPaso(string textoInstruccion, int pasoIndice)
    {
        // 1. FASE DE EXPLICACIÓN (Bloqueamos todo)
        player.enabledControl = false;
        player.ResetMovementState(); // Detiene al jugador y la animación
        player.cameraEnabled = false;

        // 1A. CAMBIO DE CÁMARA (Usamos Corrutina para esperar el blend)
        yield return StartCoroutine(MoverCamaraRoutine());

        tutorialText.text = textoInstruccion + "\n(Presiona R para continuar)";

        // 1B. ESPERA DEL JUGADOR (Tu lógica de Input original para 'R')
        yield return new WaitUntil(() => defaultActions.FindAction("WaitUntil").triggered);

        tutorialText.text = "";

        // 1C. VOLVER A CÁMARA JUGADOR (Usamos Corrutina para esperar el blend)
        yield return StartCoroutine(VolverCamaraJugadorRoutine());

        // 2. FASE DE ACCIÓN (Habilitamos el control)
        player.cameraEnabled = true;
        player.enabledControl = true; // El control está habilitado para que el jugador pueda moverse después

        tutorialText.text = textoInstruccion;

        bool accionCompletada = false;

        // >> LÓGICA CLAVE: SALTAR LA VERIFICACIÓN DE MOVIMIENTO (PASO 0) <<
        if (pasoIndice == 0)
        {
            accionCompletada = true; // Si es el primer paso, se considera completado al presionar 'R'
        }

        // 2B. BUCLE DE ACCIÓN (Solo se ejecuta si accionCompletada es false)
        while (!accionCompletada)
        {
            switch (pasoIndice)
            {
                // El Case 0 de movimiento se omite aquí debido a la línea de arriba.
                case 1: // Recoger (E)
                    if (player.HasPickedSomething()) accionCompletada = true;
                    break;
                case 2: // Limpiar (E)
                    if (player.HasCleanedTable()) accionCompletada = true;
                    break;
                case 3: // Inventario (TAB)
                    if (PlayerMove.isInventoryOpen) accionCompletada = true;
                    break;
                case 4: // Pausa (ESC)
                    if (player._input != null && player._input.pause) accionCompletada = true;
                    break;
            }
            yield return null;
        }

        // 3. PASO COMPLETADO
        tutorialText.text = "¡Bien hecho!";
        yield return new WaitForSeconds(1f);
    }

    // Corrutina para cambiar a cámara NPC y esperar el blend (Usando SetActive)
    IEnumerator MoverCamaraRoutine()
    {
        if (playerCam != null) playerCam.gameObject.SetActive(false);
        if (npcCam != null) npcCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    // Corrutina para volver a cámara Jugador y esperar el blend (Usando SetActive)
    IEnumerator VolverCamaraJugadorRoutine()
    {
        if (playerCam != null) playerCam.gameObject.SetActive(true);
        if (npcCam != null) npcCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
}