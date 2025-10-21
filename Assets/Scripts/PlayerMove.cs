using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform playerTr;
    Rigidbody playerRb;
    Animator playerAnim;

    public float playerSpeed = 0f;

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
    public float cameraSpeed = 500f;

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
    }

    public void AnimLogic()
    {
        playerAnim.SetFloat("X", newDirection.x);
        playerAnim.SetFloat("Y", newDirection.y);
    }

    public void MoveLogic()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;

        newDirection = new Vector2(moveX, moveZ);

        Vector3 side = playerSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = playerSpeed * moveZ * theTime * playerTr.forward;

        Vector3 endDirection = side + forward;

        playerRb.velocity = endDirection;
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
