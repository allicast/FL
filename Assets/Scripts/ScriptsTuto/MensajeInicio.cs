using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MensajeInicio : MonoBehaviour
{
    public GameObject texto1;
    public GameObject texto2;
    public TMP_Text mensaje;
    public float tiempoVisible;

    public float delayTexto1; // tiempo antes de aparecer
    public float duracionTexto1; // tiempo visible

    public float delayTexto2;
    public float duracionTexto2;

    void Start()
    {
        StartCoroutine(OcultarMensaje());
        StartCoroutine(ControlarTextos());
    }

    IEnumerator OcultarMensaje()
    {
        mensaje.gameObject.SetActive(true);
        yield return new WaitForSeconds(tiempoVisible);
        mensaje.gameObject.SetActive(false);
    }

    IEnumerator ControlarTextos()
    {
        // TEXTO 1
        yield return new WaitForSeconds(delayTexto1);
        texto1.SetActive(true);
        yield return new WaitForSeconds(duracionTexto1);
        texto1.SetActive(false);

        // TEXTO 2
        yield return new WaitForSeconds(delayTexto2 - (delayTexto1 + duracionTexto1));
        texto2.SetActive(true);
        yield return new WaitForSeconds(duracionTexto2);
        texto2.SetActive(false);
    }
}