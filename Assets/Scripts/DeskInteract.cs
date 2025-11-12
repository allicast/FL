using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeskInteract : MonoBehaviour
{
    public TextMeshProUGUI interactText; // Referencia al texto en UI
    public float interactionDistance = 3f; // Distancia para poder interactuar
    private Transform player;

    private bool isNear = false;
    private bool isMouseOver = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isMouseOver)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= interactionDistance && !interactText.gameObject.activeSelf)
            {
                interactText.gameObject.SetActive(true);
            }
            else if (distance > interactionDistance && interactText.gameObject.activeSelf)
            {
                interactText.gameObject.SetActive(false);
            }
        }
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        isMouseOver = false;
        interactText.gameObject.SetActive(false);
    }
}
