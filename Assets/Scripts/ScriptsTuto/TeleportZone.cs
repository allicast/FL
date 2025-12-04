using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public Transform teleportDestination;
    public AudioSource teleportSound;
    public ScreenFader screenFader;
    public float fadeDuration;
    public GameObject player;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(TeleportSequence());
        }
    }
    private IEnumerator TeleportSequence()
    {
        yield return
            StartCoroutine(screenFader.FadeIn(fadeDuration));

        if (teleportSound != null) teleportSound.Play();

        player.transform.position = teleportDestination.position;

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(screenFader.FadeOut(fadeDuration));

        isTriggered = false;
    }

}
