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

    [Header("Configuraci�n")]
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
            "�Hola! Mu�vete hacia adelante con W o las flechas para comenzar.",
            0 // Paso 0: Se salta la verificaci�n de movimiento
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Mira ese objeto y presiona 'E' para recogerlo.",
            1
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Ahora limpia la mesa. Ac�rcate y presiona 'E' de nuevo.",
            2
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Puedes ver tus cosas abriendo el inventario con TAB.",
            3
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Por �ltimo, puedes pausar el juego con ESC.",
            4
        ));

        tutorialText.text = "�Tutorial Completado! Buena suerte.";
        yield return new WaitForSeconds(3f);
        tutorialText.text = "";
        tutorialActive = false;
    }

    IEnumerator EjecutarPaso(string textoInstruccion, int pasoIndice)
    {
        
        player.enabledControl = false;
        player.ResetMovementState(); 
        player.cameraEnabled = false;

        
        yield return StartCoroutine(MoverCamaraRoutine());

        tutorialText.text = textoInstruccion + "\n(Presiona R para continuar)";

        
        yield return new WaitUntil(() => defaultActions.FindAction("WaitUntil").triggered);

        tutorialText.text = "";

        
        yield return StartCoroutine(VolverCamaraJugadorRoutine());

        
        player.cameraEnabled = true;
        player.enabledControl = true; 

        tutorialText.text = textoInstruccion;

        bool accionCompletada = false;

        
        if (pasoIndice == 0)
        {
            accionCompletada = true; 
        }

        
        while (!accionCompletada)
        {
            switch (pasoIndice)
            {
        
                case 1: 
                    if (player.HasPickedSomething()) accionCompletada = true;
                    break;
                case 2: 
                    if (player.HasCleanedTable()) accionCompletada = true;
                    break;
                case 3: 
                    if (PlayerMove.isInventoryOpen) accionCompletada = true;
                    break;
                case 4: 
                    if (player._input != null && player._input.pause) accionCompletada = true;
                    break;
            }
            yield return null;
        }

        
        tutorialText.text = "�Bien hecho!";
        yield return new WaitForSeconds(1f);
    }

    
    IEnumerator MoverCamaraRoutine()
    {
        if (playerCam != null) playerCam.gameObject.SetActive(false);
        if (npcCam != null) npcCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator VolverCamaraJugadorRoutine()
    {
        if (playerCam != null) playerCam.gameObject.SetActive(true);
        if (npcCam != null) npcCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
}