using UnityEngine;

public class NPCInteractionTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger detectó al Player");
            tutorialManager.PlayerReachedNPC();
        }
    }
}

