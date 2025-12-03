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
    public TextMeshProUGUI textOpen;
    public TextMeshProUGUI textClose;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));

        textOpen.gameObject.SetActive(false);
        textClose.gameObject.SetActive(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

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

            if (Input.GetKeyDown(KeyCode.E))
                isOpen = !isOpen;
        }
        else
        {
            textOpen.gameObject.SetActive(false);
            textClose.gameObject.SetActive(false);
        }

        Quaternion target = isOpen ? openRot : closedRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * speed);
    }
}