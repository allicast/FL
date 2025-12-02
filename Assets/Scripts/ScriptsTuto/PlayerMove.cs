using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   
    Transform playerTr;
    Rigidbody playerRb;
    Animator playerAnim;

    
    public float playerSpeed = 3f;
    public float runSpeed = 2f;
    [HideInInspector] public Vector2 newDirection;

    public Transform cameraAxis;
    public Transform cameraTrack;
    private Transform theCamera;

    public float cameraCollisionRadius = 0.2f;
    public float cameraWallOffset = 0.1f;

    public float camRotSpeed = 700f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSpeed = 700f;

    
    public bool enabledControl = true;          
    public static bool isInventoryOpen = false;  

    
    private float rotY = 0f;
    private float rotX = 0f;

    private bool isTurning = false;
    private float turnSpeed = 720f;
    private float targetAngle;
    private float startAngle;

    
    [HideInInspector] public bool isCrouching = false;

    
    private bool isInteracting = false;
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public GameObject interactText;
    private InteractableObject currentObject = null;

    
    [HideInInspector] public bool hasPickedSomething = false;  
    [HideInInspector] public bool cleanedTable = false;        

    void Start()
    {
        playerTr = transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        theCamera = Camera.main.transform;

        if (interactText != null)
            interactText.SetActive(false);
    }

    void Update()
    {
       
        if (!enabledControl)
        {
            playerRb.velocity = Vector3.zero;
            if (playerAnim != null)
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 0);
            }
            return;
        }

        if (isInventoryOpen)
        {
            playerRb.velocity = Vector3.zero;
            if (playerAnim != null)
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 0);
            }
            return;
        }

        if (isInteracting) return;

        CrouchLogic();
        DetectInteractable();
        HandleInteraction();
        MoveLogic();
        CameraLogic();
        AnimLogic();
        TurnLogic();

        
        if (Input.GetKeyDown(KeyCode.C))
        {
            
            cleanedTable = true;
           
            if (playerAnim != null) playerAnim.SetTrigger("Clean");
        }
    }

    void CrouchLogic()
    {
        isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (playerAnim != null) playerAnim.SetBool("isCrouching", isCrouching);
    }

    public void AnimLogic()
    {
        if (playerAnim == null) return;
        playerAnim.SetFloat("X", newDirection.x);
        playerAnim.SetFloat("Y", newDirection.y);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        playerAnim.SetBool("isRunning", isRunning);
    }

    public void MoveLogic()
    {
        if (isCrouching)
        {
            playerRb.velocity = Vector3.zero;
            newDirection = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveZ < 0) moveZ = 0;

        float theTime = Time.deltaTime;
        newDirection = new Vector2(moveX, moveZ);

        bool isTryingToRun = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isRunning = isTryingToRun && moveZ > 0 && !isTurning;
        float currentSpeed = isRunning ? playerSpeed * runSpeed : playerSpeed;

        Vector3 side = currentSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = Vector3.zero;

        if (!isTurning)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                isTurning = true;
                startAngle = playerTr.eulerAngles.y;
                targetAngle = (startAngle + 180f) % 360f;
            }
            else
            {
                forward = currentSpeed * moveZ * theTime * playerTr.forward;
            }
        }

        Vector3 endDirection = side + forward;
        playerRb.velocity = endDirection;
    }

    void TurnLogic()
    {
        if (!isTurning) return;

        float currentY = playerTr.eulerAngles.y;
        float newY = Mathf.MoveTowardsAngle(currentY, targetAngle, turnSpeed * Time.deltaTime);

        playerTr.eulerAngles = new Vector3(playerTr.eulerAngles.x, newY, playerTr.eulerAngles.z);

        if (Mathf.Approximately(newY, targetAngle))
            isTurning = false;
    }

    public void CameraLogic()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float theTime = Time.deltaTime;

        rotY += mouseY * theTime * camRotSpeed;
        rotX = mouseX * theTime * camRotSpeed;

        playerTr.Rotate(0, rotX, 0);

        rotY = Mathf.Clamp(rotY, minAngle, maxAngle);
        Quaternion localRotation = Quaternion.Euler(-rotY, 0, 0);
        if (cameraAxis != null) cameraAxis.localRotation = localRotation;

        if (theCamera == null) theCamera = Camera.main.transform;
        theCamera.position = cameraTrack.position;
        theCamera.rotation = cameraTrack.rotation;

        Vector3 camDir = (theCamera.position - cameraAxis.position).normalized;
        float maxDist = Vector3.Distance(cameraAxis.position, cameraTrack.position);

        if (Physics.SphereCast(
            cameraAxis.position,
            cameraCollisionRadius,
            camDir,
            out RaycastHit hit,
            maxDist
        ))
        {
            theCamera.position = hit.point - camDir * cameraWallOffset;
        }
    }

    void DetectInteractable()
    {
        if (theCamera == null) theCamera = Camera.main.transform;
        Ray ray = theCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
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
        if (currentObject != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(InteractRoutine());
        }
    }

    IEnumerator InteractRoutine()
    {
        isInteracting = true;
        playerRb.velocity = Vector3.zero;
        if (playerAnim != null) playerAnim.SetTrigger("PickUp");

        if (interactText != null) interactText.SetActive(false);

        yield return new WaitForSeconds(0.05f);

        
        if (currentObject != null)
        {
            currentObject.OnInteract();

            
            if (currentObject.gameObject.CompareTag("Pickup"))
            {
                hasPickedSomething = true;
            }

           
            if (currentObject.gameObject.CompareTag("Table"))
            {
                cleanedTable = true;
            }
        }

        yield return new WaitForSeconds(0.5f);
        isInteracting = false;
    }

    
    public bool HasPickedSomething() { return hasPickedSomething; }
    public bool HasCleanedTable() { return cleanedTable; }

    public void ResetTutorialFlags()
    {
        hasPickedSomething = false;
        cleanedTable = false;
    }
}

