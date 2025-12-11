using UnityEngine;

public class Llave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameFlags.tieneLlave = true;
            Destroy(gameObject);
        }
    }
}
