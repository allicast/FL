using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // solo si usas TextMeshPro

public class MensajeInicio : MonoBehaviour
{
    public TMP_Text mensaje;   // arrastra aquí el texto
    public float tiempoVisible = 5f; // segundos que durará

    void Start()
    {
        StartCoroutine(OcultarMensaje());
    }

    IEnumerator OcultarMensaje()
    {
        mensaje.gameObject.SetActive(true);
        yield return new WaitForSeconds(tiempoVisible);
        mensaje.gameObject.SetActive(false);
    }
}