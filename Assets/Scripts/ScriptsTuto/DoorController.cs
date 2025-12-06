using UnityEngine;

public class DoorController : BaseInteractable
{
    [Header("Configuración")]
    public float openAngle;
    public float speed = 2f;

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
    }
}
