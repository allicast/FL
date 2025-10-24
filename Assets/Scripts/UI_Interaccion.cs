using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Interaccion : MonoBehaviour
{
    public GameObject panelObjeto;
    public Image objetoImagen;
    public TMP_Text objetoNombre;

    public static UI_Interaccion instance;

    void Awake()
    {
        instance = this;
        panelObjeto.SetActive(false);
    }

    public void MostrarObjeto(Sprite imagen, string nombre)
    {
        objetoImagen.sprite = imagen;
        objetoNombre.text = nombre;
        panelObjeto.SetActive(true);
    }

    public void OcultarObjeto()
    {
        panelObjeto.SetActive(false);
    }

    void Update()
    {
        // 🔹 Si el panel está activo y se hace clic izquierdo, se oculta
        if (panelObjeto.activeSelf && Input.GetMouseButtonDown(0))
        {
            OcultarObjeto();
        }
    }
}