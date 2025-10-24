using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float camRotSpeed = 500f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSpeed = 500f;

    private bool isTurning = false;
    private float turnSpeed = 720f;
    private float targetAngle;
    private float startAngle;

    // 🔹 NUEVO: variable para saber si está agachado
    private bool isCrouching = false;

    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        theCamera = Camera.main.transform;
    }

    void Update()
    {
        CrouchLogic();   // 🔹 primero, para saber si está agachado
        MoveLogic();
        CameraLogic();
        AnimLogic();
        TurnLogic();
    }

    void CrouchLogic()
    {
        // 🔹 Si se mantiene presionado Control Izquierdo
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        // 🔹 Actualiza animación
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
        // 🔹 Si está agachado, no puede moverse
        if (isCrouching)
        {
            playerRb.velocity = Vector3.zero;
            newDirection = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;

        newDirection = new Vector2(moveX, moveZ);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float currentSpeed = isRunning ? playerSpeed * runSpeed : playerSpeed;

        Vector3 side = currentSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = Vector3.zero;

        if (!isTurning)
        {
            if (Input.GetKey(KeyCode.S))
            {
                isTurning = true;
                startAngle = playerTr.eulerAngles.y;
                targetAngle = startAngle + 180f;
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
        {
            isTurning = false;
        }
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

        // 👇 cámara siempre igual al cameraTrack
        theCamera.position = cameraTrack.position;
        theCamera.rotation = cameraTrack.rotation;
    }
}