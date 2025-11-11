using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoInteractivo : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelPuzzle;
    public GameObject textoInteractuar;
    public GameObject objetoActual;
    public GameObject objetoNuevo;

    private bool cursorEncima = false;
    private bool puzzleActivo = false;

    void Update()
    {
        if (cursorEncima && !puzzleActivo && Input.GetKeyDown(KeyCode.I))
        {
            ActivarPanelPuzzle();
        }
    }

    void ActivarPanelPuzzle()
    {
        puzzleActivo = true;
        panelPuzzle.SetActive(true);
        textoInteractuar.SetActive(false);
    }

    public void CompletarPuzzle()
    {
        objetoActual.SetActive(false);
        objetoNuevo.SetActive(true);
        panelPuzzle.SetActive(false);
        puzzleActivo = false;
    }

    public void FallarPuzzle()
    {
        panelPuzzle.SetActive(false);
        puzzleActivo = false;
        textoInteractuar.SetActive(true);
    }

    // 👉 Estos dos métodos hacen la magia del hover del mouse
    void OnMouseEnter()
    {
        if (!puzzleActivo)
        {
            textoInteractuar.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        textoInteractuar.SetActive(false);
    }
}