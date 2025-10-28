using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Datos del objeto")]
    public string objectName = "Objeto misterioso";
    public Sprite objectImage;
    [TextArea(2, 4)] public string objectDescription = "Una descripción del objeto.";

    [HideInInspector] public bool isFocused = false;

    public void OnFocus()
    {
        isFocused = true;
    }

    public void OnLoseFocus()
    {
        isFocused = false;
    }

    public void OnInteract()
    {
        Debug.Log("Has recogido " + objectName);

        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.AddItem(objectImage, objectName);
        }

        if (UI_Interaccion.instance != null)
        {
            UI_Interaccion.instance.MostrarObjeto(objectImage, objectName, objectDescription);
        }

        gameObject.SetActive(false);
    }
}
