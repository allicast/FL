using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] LayerMask layers;
    [SerializeField] GameObject interactText;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        bool interactablePointed = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layers, QueryTriggerInteraction.Ignore);
        if (interactablePointed)
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
            {
                interactText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
            else
            {
                interactText.SetActive(false);
            }
        }
        else
        {
            interactText.SetActive(false);
        }          
    }
}
