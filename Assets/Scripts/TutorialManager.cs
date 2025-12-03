using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("UI y Referencias")]
    public TextMeshProUGUI tutorialText;
    public Transform npcLookTarget;
    public PlayerMove player; 

    [Header("Configuración")]
    public float camRotateSpeed = 3f;

    // Estado del tutorial
    public bool tutorialActive = false;
    private int currentStep = 0;
    private Transform mainCam;

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
            "Hola Faver. dejame explicarte algunas cosas que debes de saber, empieza moviendote con ASWD.", 
            0 
        ));

        
        yield return StartCoroutine(EjecutarPaso(
            "Muy bien. Ahora intenta agacharte presionando CTRL.",
            1
        ));

        
        yield return StartCoroutine(EjecutarPaso(
            "Excelente. Acércate y recoge algun objeto presionando E.",
            2
        ));

       
        yield return StartCoroutine(EjecutarPaso(
            "Ahora limpia la mesa usando la tecla C.",
            3
        ));

        
        yield return StartCoroutine(EjecutarPaso(
            "Puedes ver tus cosas abriendo el inventario con TAB.",
            4
        ));

        
        yield return StartCoroutine(EjecutarPaso(
            "Por último, puedes pausar el juego con ESC.",
            5
        ));

        
        tutorialText.text = "¡Tutorial Completado Faver! Te espero abajo.";
        yield return new WaitForSeconds(3f);
        tutorialText.text = "";
        tutorialActive = false;
    }

    
    IEnumerator EjecutarPaso(string textoInstruccion, int pasoIndice)
    {
        
        player.enabledControl = false;
        player.ResetMovementState(); 
        player.cameraEnabled = false; 

        yield return StartCoroutine(MoverCamara(npcLookTarget));

        
        tutorialText.text = textoInstruccion + "\n(Presiona R para continuar)";

     
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));

        
        tutorialText.text = ""; 

        
        yield return StartCoroutine(VolverCamaraAJugador());

        
        player.cameraEnabled = true;
        player.enabledControl = true;

        
        tutorialText.text = textoInstruccion;

        bool accionCompletada = false;

        
        while (!accionCompletada)
        {
            switch (pasoIndice)
            {
                case 0: 
                    if (player.newDirection.magnitude > 0.1f) accionCompletada = true;
                    break;
                case 1: 
                    if (player.isCrouching) accionCompletada = true;
                    break;
                case 2: 
                    if (player.HasPickedSomething()) accionCompletada = true;
                    break;
                case 3: 
                    if (player.HasCleanedTable()) accionCompletada = true;
                    break;
                case 4: 
                    if (PlayerMove.isInventoryOpen) accionCompletada = true;
                    break;
                case 5: 
                    if (Input.GetKeyDown(KeyCode.Escape)) accionCompletada = true;
                    break;
            }
            yield return null; 
        }

        
        tutorialText.text = "¡Bien hecho!";
        yield return new WaitForSeconds(1f);
    }

    
    IEnumerator MoverCamara(Transform objetivo)
    {
        Quaternion rotacionInicial = mainCam.rotation;
        Quaternion rotacionFinal = Quaternion.LookRotation(objetivo.position - mainCam.position);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * camRotateSpeed;
            mainCam.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, t);
            yield return null;
        }
        mainCam.rotation = rotacionFinal;
    }

    
    IEnumerator VolverCamaraAJugador()
    {
        Quaternion rotacionInicial = mainCam.rotation;
        
        Quaternion rotacionFinal = player.playerCameraPivot.rotation;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * camRotateSpeed;
            mainCam.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, t);
            yield return null;
        }
        
    }
}