using UnityEngine;

public class DoorControllers : BaseInteractable
{
    [Header("Configuración")]
    public float openAngle;
    public float speed = 2f;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip soundOpen;
    public AudioClip soundClose;

    private bool isOpen = false;
    private bool playerInRange = false;

    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    void Update()
    {
        // Si está cerca y presiona E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        Quaternion target = isOpen ? openRot : closedRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    public override void Interact()
    {
        // ?? No tiene la llave ? no abre
        if (!GameFlags.tieneLlave)
        {
            Debug.Log("No tienes la llave.");
            return;
        }

        // ?? Tiene la llave ? abrir/cerrar
        isOpen = !isOpen;

        if (audioSource == null) return;

        if (isOpen && soundOpen != null)
            audioSource.PlayOneShot(soundOpen);
        else if (!isOpen && soundClose != null)
            audioSource.PlayOneShot(soundClose);
    }
}
