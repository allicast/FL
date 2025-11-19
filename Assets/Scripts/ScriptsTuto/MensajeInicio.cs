using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MensajeInicio : MonoBehaviour
{
    public TMP_Text mensaje;
    public float tiempoVisible;

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