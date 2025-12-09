using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinematicaInicio : MonoBehaviour
{
    public float cinematicDuration; // tiempo total

    void Start()
    {
        Debug.Log("Cinemática iniciada");
        StartCoroutine(CinematicRoutine());
    }

    IEnumerator CinematicRoutine()
    {
        yield return new WaitForSeconds(cinematicDuration);

        Debug.Log("Cargando escena Dialogosinicio...");
        SceneManager.LoadScene(2);
    }
}


