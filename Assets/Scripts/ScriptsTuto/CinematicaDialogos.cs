using UnityEngine;
using TMPro;
using System.Collections;

public class CinematicaDialogos : MonoBehaviour
{
    public TextMeshProUGUI uiText;       // Texto UI
    public string message;               // Texto a escribir
    public float typingSpeed = 0.03f;    // Velocidad entre letras
    public AudioSource typingSound;      // Sonido tecla
    public float startDelay = 1f;        // ⬅ Tiempo antes de comenzar (editable)

    void Start()
    {
        StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {
        // Espera antes de empezar a escribir
        yield return new WaitForSeconds(startDelay);

        uiText.text = "";

        foreach (char c in message)
        {
            uiText.text += c;

            if (typingSound != null)
                typingSound.Play();

            yield return new WaitForSeconds(typingSpeed);
        }

        // Ocultar inmediatamente al terminar
        uiText.gameObject.SetActive(false);
    }
}

