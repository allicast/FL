using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Transform playerTr;
    Rigidbody playerRb;
    Animator playerAnim;

    public float playerSpeed;
    public float runSpeed = 2f;
    private Vector2 newDirection;

    public Transform cameraAxis;
    public Transform cameraTrack;
    private Transform theCamera;

    private float rotY = 0f;
    private float rotX = 0f;

    public float camRotSpeed = 700f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSpeed = 700f;

    private bool isTurning = false;
    private float turnSpeed = 720f;
    private float targetAngle;
    private float startAngle;

    private bool isCrouching = false;
    private bool isInteracting = false;

    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public GameObject interactText;
    private InteractableObject currentObject = null;

    public static bool isInventoryOpen = false;

    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        theCamera = Camera.main.transform;

        if (interactText != null)
            interactText.SetActive(false);
    }

    void Update()
    {
        if (isInventoryOpen)
        {
            playerRb.velocity = Vector3.zero;
            playerAnim.SetFloat("X", 0);
            playerAnim.SetFloat("Y", 0);
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
    }

    void CrouchLogic()
    {
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        playerAnim.SetBool("isCrouching", isCrouching);
    }

    public void AnimLogic()
    {
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
        cameraAxis.localRotation = localRotation;

        theCamera.position = cameraTrack.position;
        theCamera.rotation = cameraTrack.rotation;
    }

    void DetectInteractable()
    {
        Ray ray = theCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            InteractableObject obj = hit.collider.GetComponent<InteractableObject>();
            if (obj != null)
            {
                currentObject = obj;
                if (interactText != null)
                    interactText.SetActive(true);
                return;
            }
        }

        currentObject = null;
        if (interactText != null)
            interactText.SetActive(false);
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
        playerAnim.SetTrigger("PickUp");

        if (interactText != null)
            interactText.SetActive(false);

        yield return new WaitForSeconds(0.05f);

        currentObject.OnInteract();

        yield return new WaitForSeconds(5.5f);
        isInteracting = false;
    }
}