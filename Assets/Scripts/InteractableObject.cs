using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Datos del objeto")]
    public string objectName = "Objeto misterioso";
    public Sprite objectImage; // imagen para mostrar en el canvas

    [HideInInspector] public bool isFocused = false;

    public void OnFocus()
    {
        isFocused = true;
    }

    public void OnLoseFocus()
    {
        isFocused = false;
    }

    // Se llama cuando el jugador presiona "E"
    public void OnInteract()
    {
        // Aquí solo notificamos al Player que fue interactuado
        Debug.Log("Has recogido " + objectName);
        if (UI_Interaccion.instance != null)
        {
            UI_Interaccion.instance.MostrarObjeto(objectImage, objectName);
        }

        gameObject.SetActive(false);
    }
}
