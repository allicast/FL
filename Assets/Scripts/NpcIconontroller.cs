using UnityEngine;
using UnityEngine.UI;

public class NPCIconController : MonoBehaviour
{
    public Image icon;
    public TutorialManager tutorialManager;

    void Update()
    {
        if (tutorialManager == null) return;

        icon.enabled = !tutorialManager.tutorialActive;
    }
}
