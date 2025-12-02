using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public Text tutorialText;         
    public Camera mainCam;            
    public Transform camOriginalPos;  
    public Transform camNPCPos;       
    public PlayerMove player;        

    public float camMoveSpeed = 2f;
    public float waitAfterReturn = 0.5f;

    public bool TutorialStarted = false;
    int currentStep = 0;
    bool waitingPlayerAction = false;

    
    public void PlayerReachedNPC()
    {
        if (!TutorialStarted)
        {
            TutorialStarted = true;
            StartCoroutine(StartTutorial());
        }
    }

    IEnumerator StartTutorial()
    {
        
        if (mainCam == null) mainCam = Camera.main;

        
        yield return new WaitForSeconds(0.1f);

        
        yield return MoveCameraTo(camNPCPos.position, camNPCPos.rotation);
        StartStep();
    }

    void StartStep()
    {
        waitingPlayerAction = false;

        
        if (player != null) player.ResetTutorialFlags();

        switch (currentStep)
        {
            case 0:
                tutorialText.text = "Muévete con W A S D.";
                waitingPlayerAction = true;
                break;

            case 1:
                tutorialText.text = "Agáchate presionando CTRL.";
                waitingPlayerAction = true;
                break;

            case 2:
                tutorialText.text = "Recoge un objeto presionando E (usa un objeto con tag \"Pickup\").";
                waitingPlayerAction = true;
                break;

            case 3:
                tutorialText.text = "Limpia la mesa usando C (o interactúa con una mesa con tag \"Table\").";
                waitingPlayerAction = true;
                break;

            case 4:
                tutorialText.text = "Abre tu inventario con TAB.";
                waitingPlayerAction = true;
                break;

            case 5:
                tutorialText.text = "Puedes pausar presionando ESC.";
                waitingPlayerAction = true;
                break;

            default:
                tutorialText.text = "¡Tutorial completado!";
                waitingPlayerAction = false;
                break;
        }

        
        if (player != null) player.enabledControl = true;
    }

    void Update()
    {
        if (!TutorialStarted || !waitingPlayerAction || player == null) return;

        switch (currentStep)
        {
            case 0:
                if (player.newDirection.magnitude > 0.1f)
                    CompleteStep();
                break;

            case 1:
                if (player.isCrouching)
                    CompleteStep();
                break;

            case 2:
                
                if (player.HasPickedSomething())
                    CompleteStep();
                break;

            case 3:
                if (player.HasCleanedTable())
                    CompleteStep();
                break;

            case 4:
                if (PlayerMove.isInventoryOpen)
                    CompleteStep();
                break;

            case 5:
                if (Input.GetKeyDown(KeyCode.Escape))
                    CompleteStep();
                break;
        }
    }

    void CompleteStep()
    {
        waitingPlayerAction = false;
        
        if (player != null) player.enabledControl = false;
        StartCoroutine(ReturnToPlayerThenContinue());
    }

    IEnumerator ReturnToPlayerThenContinue()
    {
        
        yield return MoveCameraTo(camOriginalPos.position, camOriginalPos.rotation);

        yield return new WaitForSeconds(waitAfterReturn);

        
        currentStep++;

        if (currentStep <= 5)
        {
            
            yield return MoveCameraTo(camNPCPos.position, camNPCPos.rotation);
            StartStep();
        }
        else
        {
            
            tutorialText.text = "";
            if (player != null) player.enabledControl = true;
        }
    }

    IEnumerator MoveCameraTo(Vector3 targetPos, Quaternion targetRot)
    {
        float t = 0f;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        float duration = 1f / camMoveSpeed; // control sencillo
        if (duration <= 0) duration = 0.1f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, t));
            mainCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.transform.rotation = targetRot;
    }
}

