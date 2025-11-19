using UnityEngine;
using UnityEngine.UI;
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
        if (panelObjeto.activeSelf && Input.GetMouseButtonDown(0))
        {
            OcultarObjeto();
        }

        if (panelTextoUso.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                panelTextoUso.GetComponent<RectTransform>(),
                Input.mousePosition,
                null))
            {
                CerrarTextoUso();
            }
        }
    }
}