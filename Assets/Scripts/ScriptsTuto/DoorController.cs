using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    [Header("Configuración")]
    public float openAngle;
    public float speed = 2f;

    [Header("Distancia para mostrar texto")]
    public float detectionDistance;
    public Transform player;

    [Header("UI")]
    public TextMeshProUGUI textOpen;   // "Presiona E para abrir"
    public TextMeshProUGUI textClose;  // "Presiona E para cerrar"

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));

        // Ocultar textos al iniciar
        textOpen.gameObject.SetActive(false);
        textClose.gameObject.SetActive(false);
    }

    void Update()
    {
        // Distancia entre jugador y puerta
        float dist = Vector3.Distance(player.position, transform.position);

        // Mostrar texto solo si está cerca
        if (dist <= detectionDistance)
        {
            if (isOpen)
            {
                textClose.gameObject.SetActive(true);
                textOpen.gameObject.SetActive(false);
            }
            else
            {
                textOpen.gameObject.SetActive(true);
                textClose.gameObject.SetActive(false);
            }

            // Si presiona E, cambiar estado
            if (Input.GetKeyDown(KeyCode.E))
                isOpen = !isOpen;
        }
        else
        {
            // Ocultar ambos textos si está lejos
            textOpen.gameObject.SetActive(false);
            textClose.gameObject.SetActive(false);
        }

        // Rotación suave
        Quaternion target = isOpen ? openRot : closedRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * speed);
    }
}