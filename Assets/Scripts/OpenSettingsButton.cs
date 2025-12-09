using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettingsButton : MonoBehaviour
{
    public GameObject settingsPanel;   // Panel con los botones de controles y sonido
    public GameObject pausePanel;      // Panel de pausa

    public void OpenSettings()
    {
        pausePanel.SetActive(false);     // Oculta el panel de pausa
        settingsPanel.SetActive(true);   // Muestra el panel de configuración
    }
}