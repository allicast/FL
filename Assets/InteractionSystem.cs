using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] LayerMask layers;
    private PlayerMove player;
    [SerializeField] GameObject interactText;
    public InputActionAsset defaultActions;
    private GameObject lastPickedObject;
    private DeskPuzzle deskPuzzle;

    private void Start()
    {
        player = GetComponent<PlayerMove>();

    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        bool interactablePointed = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layers, QueryTriggerInteraction.Ignore);
        if (interactablePointed)
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
            {
                interactText.SetActive(true);
                if (defaultActions.FindAction("Interact").WasPressedThisFrame())
                {
                    interactable.Interact();
                    lastPickedObject = hit.collider.gameObject;
                    switch(lastPickedObject.GetComponent<DeskPuzzle>())
                    {
                        case null:
                            Debug.Log("No es el puzzle de limpiar");
                            break;
                        case DeskPuzzle:
                            deskPuzzle = lastPickedObject.GetComponent<DeskPuzzle>();
                            break;
                    }
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
            lastPickedObject = null;
        }          
    }
    public bool HasPickedSomething()
    {
        if (lastPickedObject != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool HasCleanedTable()
    {
        if(deskPuzzle != null)
        {
            return deskPuzzle.activityIsCompleted;
        }
        else
        {
            return false;
        }
    }
}
