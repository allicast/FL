using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agachado: MonoBehaviour
{
    public float speed = 3f;
    public float crouchSpeed = 1.5f;

    Animator anim;
    Rigidbody rb;
    Transform cam;

    float horizontal;
    float vertical;
    bool isCrouching;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        // Movimiento
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Detección de agachado (mientras se mantiene CTRL presionado)
        if (Input.GetKey(KeyCode.LeftControl))
            isCrouching = true;
        else
            isCrouching = false;

        // Enviar al Animator
        anim.SetBool("isCrouching", isCrouching);
        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);

        // Movimiento físico
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetSpeed = isCrouching ? crouchSpeed : speed;
            Vector3 moveDir = cam.forward * direction.z + cam.right * direction.x;
            moveDir.y = 0;
            rb.MovePosition(transform.position + moveDir * targetSpeed * Time.deltaTime);
        }
    }
}