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
    public float runSpeed = 2f; // multiplicador para correr (ejemplo 2x)

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

    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();

        theCamera = Camera.main.transform;
    }

    void Update()
    {
        MoveLogic();
        CameraLogic();
        AnimLogic();
        TurnLogic();
    }

    public void AnimLogic()
    {
        playerAnim.SetFloat("X", newDirection.x);
        playerAnim.SetFloat("Y", newDirection.y);

        // Nuevo: Animación de correr con Shift
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        playerAnim.SetBool("isRunning", isRunning);
    }

    public void MoveLogic()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;

        newDirection = new Vector2(moveX, moveZ);

        // Checar si se está corriendo (Shift)
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Aplicar multiplicador si se corre
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

        theCamera.position = Vector3.Lerp(theCamera.position, cameraTrack.position, cameraSpeed * theTime);
        theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraTrack.rotation, cameraSpeed * theTime);
    }
}
