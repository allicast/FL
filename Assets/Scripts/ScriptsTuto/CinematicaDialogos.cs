using UnityEngine;
using TMPro;
using System.Collections;

public class CinematicaDialogos : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public string message;
    public float typingSpeed = 0.03f;
    public AudioSource typingSound;
    public float startDelay = 1f;

    void Start()
    {
        StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {

        yield return new WaitForSeconds(startDelay);

        uiText.text = "";

        foreach (char c in message)
        {
            uiText.text += c;

            if (typingSound != null)
                typingSound.Play();

            yield return new WaitForSeconds(typingSpeed);
        }


        uiText.gameObject.SetActive(false);
    }
}

