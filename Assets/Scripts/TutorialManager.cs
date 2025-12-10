using Cinemachine;
using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [Header("UI y Referencias")]
    public TextMeshProUGUI tutorialText;
    public Image bgText;
    [SerializeField] Color offColor;
    [SerializeField] Color onColor;
    public Transform npcLookTarget;
    public InteractionSystem player;
    public ThirdPersonController playerControl;
    public CinemachineVirtualCamera playerCam, npcCam;

    [Header("Configuraci n")]
    public float camRotateSpeed = 3f;

    private bool tutorialActive = false;
    private Transform mainCam;
    public InputActionAsset defaultActions;

    void Start()
    {


        Debug.Log("TM Player asignado: " + player);

        mainCam = Camera.main.transform;
        tutorialText.text = "";
        bgText.color = offColor;
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
            " Hola! Muevete hacia adelante con W o las flechas para comenzar.",
            0
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Mira ese objeto y presiona 'E' para recogerlo.",
            1
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Ahora limpia la mesa. Ac rcate y presiona 'E' de nuevo.",
            2
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Puedes ver tus cosas abriendo el inventario con TAB.",
            3
        ));

        yield return StartCoroutine(EjecutarPaso(
            "Por  ltimo, puedes pausar el juego con ESC.",
            4
        ));

        tutorialText.text = " Tutorial Completado! Buena suerte.";
        yield return new WaitForSeconds(3f);
        tutorialText.text = "";
        bgText.color= offColor;
        tutorialActive = false;
    }

    IEnumerator EjecutarPaso(string textoInstruccion, int pasoIndice)
    {

        yield return StartCoroutine(MoverCamaraRoutine());

        tutorialText.text = textoInstruccion + "\n(Presiona R para continuar)";
        bgText.color = onColor;


        yield return new WaitUntil(() => defaultActions.FindAction("WaitUntil").triggered);

        tutorialText.text = "";
        bgText.color = offColor;


        yield return StartCoroutine(VolverCamaraJugadorRoutine());

        tutorialText.text = textoInstruccion;
        bgText.color = onColor;

        bool accionCompletada = false;





        while (!accionCompletada)
        {
            switch (pasoIndice)
            {
                case 0: // Esperar a que el jugador se mueva (entrada de movimiento > 0)
                    if (defaultActions.FindAction("Move").ReadValue<Vector2>() != Vector2.zero) accionCompletada = true;
                        break;
                case 1:
                    Debug.Log("Esperando a que el jugador recoja algo... estado: " + player.HasPickedSomething());
                    if (player.HasPickedSomething()) accionCompletada = true;
                    break;

                case 2:
                    if (player.HasCleanedTable()) accionCompletada = true;
                    break;

                case 3:
                    if (PlayerMove.isInventoryOpen) accionCompletada = true;
                    break;

                case 4:
                    if (defaultActions.FindAction("Pause").WasPressedThisFrame()) accionCompletada = true;
                    break;
            }
            yield return null;
        }


        tutorialText.text = " Bien hecho!";
        yield return new WaitForSeconds(1f);
    }


    IEnumerator MoverCamaraRoutine()
    {
        if (playerCam != null)
        {
            playerCam.gameObject.SetActive(false);
            playerControl.enabled=false;
        }
        if (npcCam != null) npcCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator VolverCamaraJugadorRoutine()
    {
        if (playerCam != null)
        {
            playerCam.gameObject.SetActive(true);
            playerControl.enabled = true;
        }
        if (npcCam != null) npcCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

}