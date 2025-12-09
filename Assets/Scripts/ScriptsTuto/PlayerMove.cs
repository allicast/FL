using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerMove : MonoBehaviour
{
    public StarterAssetsInputs _input;

    // Variables de control para el Tutorial
    [HideInInspector] public bool enabledControl = true;
    [HideInInspector] public bool cameraEnabled = true;
    public Transform playerCameraPivot;

    // Estados internos
    private bool hasPickedItem = false;
    private bool hasCleaned = false;

    Transform playerTr;
    Rigidbody playerRb;
    Animator playerAnim;

    public float playerSpeed;
    public float runSpeed = 2f;
    [HideInInspector]
    public Vector2 newDirection;

    public Transform cameraAxis;
    public Transform cameraTrack;
    private Transform theCamera;

    public float cameraCollisionRadius = 0.2f;
    public float cameraWallOffset = 0.1f;

    private float rotY = 0f;
    private float rotX = 0f;

    public float camRotSpeed = 700f;
    public float minAngle = -45f;
    public float maxAngle = 45f;

    private bool isTurning = false;
    private float turnSpeed = 720f;
    private float targetAngle;

    private bool isInteracting = false;

    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public GameObject interactText;
    private InteractableObject currentObject = null;

    public static bool isInventoryOpen = false;

    // --- SOLUCIÓN: Usar solo Awake para la inicialización más temprana ---
    void Awake()
    {
        playerTr = this.transform;

        // Asignación de componentes con comprobación de error
        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
            Debug.LogError("PlayerMove: ¡FALTA Rigidbody! Necesario para el movimiento.");

        playerAnim = GetComponentInChildren<Animator>();
        if (playerAnim == null)
            Debug.LogError("PlayerMove: ¡FALTA Animator! Necesario para las animaciones.");

        // Inicialización de la cámara (Camera.main puede ser nula en Awake si no está bien configurada, pero lo intentamos)
        if (Camera.main != null)
        {
            theCamera = Camera.main.transform;
        }
        else
        {
            // Esto puede ser normal si la cámara se genera tarde
            Debug.LogWarning("PlayerMove: Camera.main no está disponible en Awake. Se buscará más tarde.");
        }

        if (_input == null) _input = GetComponent<StarterAssetsInputs>();
        if (playerCameraPivot == null) playerCameraPivot = cameraAxis;

        if (interactText != null)
            interactText.SetActive(false);
    }
    

    void Update()
    {
        
        if (_input == null) return;

        isInventoryOpen = _input.inventory;

        if (isInventoryOpen)
        {
            // PROTECCIÓN
            if (playerRb != null) playerRb.velocity = Vector3.zero;
            if (playerAnim != null)
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 0);
            }
            return;
        }

        if (isInteracting) return;

        DetectInteractable();
        HandleInteraction();
        MoveLogic();
        CameraLogic();
        AnimLogic();
        TurnLogic();
    }

    public void AnimLogic()
    {
        if (playerAnim == null || _input == null) return;

        playerAnim.SetFloat("X", newDirection.x);
        playerAnim.SetFloat("Y", newDirection.y);

        bool isRunning = _input.sprint;
        playerAnim.SetBool("isRunning", isRunning);
    }

    public void MoveLogic()
    {
       
        if (_input == null)
        {
            
            newDirection = Vector2.zero;
            return;
        }

        float moveX = _input.move.x;
        float moveZ = _input.move.y;

        
        newDirection = new Vector2(moveX, moveZ);


        
        if (!enabledControl)
        {
            // Si el control está deshabilitado, forzamos la velocidad a cero.
            if (playerRb != null) playerRb.velocity = Vector3.zero;
            if (playerAnim != null)
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 0);
            }
            
            return;
        }

       
        if (moveZ < 0) moveZ = 0;

        float theTime = Time.deltaTime;

        bool isTryingToRun = _input.sprint;
        bool isRunning = isTryingToRun && moveZ > 0 && !isTurning;
        float currentSpeed = isRunning ? playerSpeed * runSpeed : playerSpeed;

        Vector3 side = currentSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = Vector3.zero;

        if (!isTurning)
        {
            forward = currentSpeed * moveZ * theTime * playerTr.forward;
        }

        Vector3 endDirection = side + forward;
        if (playerRb != null) playerRb.velocity = endDirection;
    }

    void TurnLogic()
    {
        if (!isTurning) return;
        float currentY = playerTr.eulerAngles.y;
        float newY = Mathf.MoveTowardsAngle(currentY, targetAngle, turnSpeed * Time.deltaTime);
        playerTr.eulerAngles = new Vector3(playerTr.eulerAngles.x, newY, playerTr.eulerAngles.z);
        if (Mathf.Approximately(newY, targetAngle)) isTurning = false;
    }

    public LayerMask collisionLayer;

    void CameraLogic()
    {
        if (!cameraEnabled || _input == null || cameraAxis == null) return;

        
        if (theCamera == null && Camera.main != null)
        {
            theCamera = Camera.main.transform;
        }
        if (theCamera == null) return; 

        float mouseX = _input.look.x;
        float mouseY = _input.look.y;
        float sensitivityMultiplier = 0.5f;
        float theTime = Time.deltaTime;

        rotY += mouseY * theTime * camRotSpeed * sensitivityMultiplier;
        rotX = mouseX * theTime * camRotSpeed * sensitivityMultiplier;

        playerTr.Rotate(0, rotX, 0);

        rotY = Mathf.Clamp(rotY, minAngle, maxAngle);
        cameraAxis.localRotation = Quaternion.Euler(-rotY, 0, 0);

        if (cameraTrack != null)
        {
            theCamera.position = cameraTrack.position;
            theCamera.rotation = cameraTrack.rotation;

            Vector3 camDir = (theCamera.position - cameraAxis.position).normalized;
            float maxDist = Vector3.Distance(cameraAxis.position, cameraTrack.position);

            if (Physics.SphereCast(cameraAxis.position, cameraCollisionRadius, camDir, out RaycastHit hit, maxDist, collisionLayer))
            {
                theCamera.position = hit.point - camDir * cameraWallOffset;
            }
        }
    }

    void DetectInteractable()
    {
        if (theCamera == null) return;

        Vector3 mousePos = Vector3.zero;
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        else return;
#else
        mousePos = Input.mousePosition;
#endif

        Ray ray = theCamera.GetComponent<Camera>().ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            InteractableObject obj = hit.collider.GetComponent<InteractableObject>();
            if (obj != null)
            {
                currentObject = obj;
                if (interactText != null) interactText.SetActive(true);
                return;
            }
        }

        currentObject = null;
        if (interactText != null) interactText.SetActive(false);
    }

    void HandleInteraction()
    {
        if (_input == null || !_input.interact) return;

        _input.interact = false;

        if (currentObject != null)
        {
            StartCoroutine(InteractRoutine());
        }
        else
        {
            if (!hasCleaned)
            {
                // PROTECCIÓN
                if (playerAnim != null) playerAnim.SetTrigger("Clean");
                hasCleaned = true;
            }
        }
    }

    IEnumerator InteractRoutine()
    {
        isInteracting = true;
        // PROTECCIÓN
        if (playerRb != null) playerRb.velocity = Vector3.zero;
        if (playerAnim != null) playerAnim.SetTrigger("PickUp");

        hasPickedItem = true;

        if (interactText != null) interactText.SetActive(false);

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(2.0f);
        isInteracting = false;
    }

    
   

    public bool HasPickedSomething() { return hasPickedItem; }
    public bool HasCleanedTable() { return hasCleaned; }

    public void ResetMovementState()
    {
       

        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
        }

        if (playerAnim != null)
        {
            playerAnim.SetFloat("X", 0);
            playerAnim.SetFloat("Y", 0);
        }
    }
}