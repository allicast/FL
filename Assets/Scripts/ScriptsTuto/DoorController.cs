using UnityEngine;

public class DoorController : BaseInteractable
{
    [Header("Configuración")]
    public float openAngle;
    public float speed = 2f;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip soundOpen;   // sonido cuando se abre
    public AudioClip soundClose;  // sonido cuando se cierra

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    void Update()
    {
        Quaternion target = isOpen ? openRot : closedRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * speed);
    }

    public override void Interact()
    {
        isOpen = !isOpen;

        if (audioSource == null) return;

        // Si la puerta se está abriendo → sonido abrir
        if (isOpen && soundOpen != null)
        {
            audioSource.PlayOneShot(soundOpen);
        }
        // Si la puerta se está cerrando → sonido cerrar
        else if (!isOpen && soundClose != null)
        {
            audioSource.PlayOneShot(soundClose);
        }
    }
}