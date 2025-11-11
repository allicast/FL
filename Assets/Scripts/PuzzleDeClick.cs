using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleDeClick : MonoBehaviour
{
    public Slider barraProgreso;
    public Button botonClick;
    public float tiempoLimite = 5f;
    public float progresoPorClick = 0.2f;

    private float tiempoRestante;
    private ObjetoInteractivo objetoPadre;

    void Start()
    {
        tiempoRestante = tiempoLimite;
        barraProgreso.value = 0;
        botonClick.onClick.AddListener(SumarProgreso);
        objetoPadre = FindObjectOfType<ObjetoInteractivo>();
    }

    void Update()
    {
        if (barraProgreso.value >= 1f)
        {
            objetoPadre.CompletarPuzzle();
        }

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0 && barraProgreso.value < 1f)
        {
            objetoPadre.FallarPuzzle();
        }
    }

    void SumarProgreso()
    {
        barraProgreso.value += progresoPorClick;
    }

    void OnEnable()
    {
        // Reiniciar valores cada vez que se abre el panel
        tiempoRestante = tiempoLimite;
        barraProgreso.value = 0;
    }
}
