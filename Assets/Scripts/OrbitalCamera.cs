using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitalCamera : MonoBehaviour
{
    public Transform follow;   // jugador
    public float distance = 5f; // distancia detrás del jugador
    public float height = 2f;   // altura de la cámara
    public float side = 1f;     // desplazamiento lateral (hombro)
    public float tilt = 10f;    // inclinación hacia abajo

    void LateUpdate()
    {
        if (follow != null)
        {
            // posición detrás y al lado del jugador
            Vector3 pos = follow.position - follow.forward * distance + follow.right * side;
            pos.y = follow.position.y + height;

            // mueve la cámara
            transform.position = pos;

            // mira al jugador
            transform.LookAt(follow.position + Vector3.up * 1.5f);

            // inclina la cámara ligeramente hacia abajo
            transform.RotateAround(follow.position, transform.right, tilt);
        }
    }
}