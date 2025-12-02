using UnityEngine;

public class NPCInteractionTrigger : MonoBehaviour
{
    public TutorialManager tutorial;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorial.PlayerReachedNPC();
        }
    }
}

