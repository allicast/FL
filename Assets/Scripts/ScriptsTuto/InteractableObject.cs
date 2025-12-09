using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : BaseInteractable
{
    [Header("Datos del objeto")]
    public string objectName = "Objeto misterioso";
    public Sprite objectImage;
    [TextArea(2, 4)] public string objectDescription = "Una descripción del objeto.";
    [TextArea] public string useText = "describe texto de usar";


    public override void Interact()
    {
        Debug.Log("Has recogido " + objectName);

        // --- AGREGAR AL INVENTARIO ---
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.AddItem(objectImage, objectName, useText);
        }

        // --- MOSTRAR PANEL DE OBJETO RECOGIDO ---
        if (UI_Interaccion.instance != null)
        {
            UI_Interaccion.instance.MostrarObjeto(objectImage, objectName, objectDescription);
        }

        // --- DESAPARECER EL OBJETO DEL MUNDO ---
        gameObject.SetActive(false);
    }
}
