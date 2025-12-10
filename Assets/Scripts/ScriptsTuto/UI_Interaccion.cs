using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UI_Interaccion : MonoBehaviour
{
    public GameObject panelObjeto;
    public Image objetoImagen;
    public TMP_Text objetoNombre;
    public TMP_Text objetoDescripcion;

    public static UI_Interaccion instance;

    public GameObject panelTextoUso;
    public Text textoUso;
    public InputActionAsset defaultActions;

    void Awake()
    {
        instance = this;
        panelObjeto.SetActive(false);
        panelTextoUso.SetActive(false);
    }

    public void MostrarObjeto(Sprite imagen, string nombre, string descripcion)
    {
        objetoImagen.sprite = imagen;
        objetoNombre.text = nombre;
        objetoDescripcion.text = descripcion;
        panelObjeto.SetActive(true);
    }

    public void MostrarTextoUso(string texto)
    {
        if (textoUso != null)
        {
            textoUso.text = texto;
            panelTextoUso.SetActive(true);
        }
    }
    public void OcultarObjeto()
    {
        panelObjeto.SetActive(false);
    }
    public void CerrarTextoUso()
    {
        if (panelTextoUso != null)
            panelTextoUso.SetActive(false);
    }

    void Update()
    {
        if (panelObjeto.activeSelf && defaultActions.FindAction("ClickUp").WasPressedThisFrame())
        {
            OcultarObjeto();
        }

        if (panelTextoUso.activeSelf && defaultActions.FindAction("ClickUp").WasPressedThisFrame())
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                panelTextoUso.GetComponent<RectTransform>(),
                defaultActions.FindAction("Pointer").ReadValue<Vector2>(),
                null))
            {
                CerrarTextoUso();
            }
        }
    }
}