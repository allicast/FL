using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DeskPuzzle : MonoBehaviour
{
    [Header("Referencias del Puzzle")]
    public GameObject puzzlePanel;
    public Slider progressBar;
    public Button clickButton;
    public TextMeshProUGUI timerText;

    [Header("Escritorios")]
    public GameObject messyDesk;
    public GameObject organizedDesk;

    [Header("Configuración del Puzzle")]
    public float timeLimit = 5f;
    public float progressPerClick = 0.1f;

    private bool isPuzzleActive = false;
    private float currentTime;

    void Start()
    {
        puzzlePanel.SetActive(false);
        progressBar.value = 0f;
        clickButton.onClick.AddListener(OnClickButton);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isPuzzleActive)
        {
            float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position);
            if (distance <= 3f)
            {
                StartPuzzle();
            }
        }
    }

    void StartPuzzle()
    {
        isPuzzleActive = true;
        currentTime = timeLimit;
        progressBar.value = 0f;
        puzzlePanel.SetActive(true);

        // 🔹 Mostrar el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 🔹 PONER EN PAUSA EL JUEGO
        Time.timeScale = 0f;

        // 🔹 Iniciar el temporizador usando tiempo real (no afectado por la pausa)
        StartCoroutine(PuzzleTimer());
    }

    IEnumerator PuzzleTimer()
    {
        float realTimeStart = Time.realtimeSinceStartup;
        while ((Time.realtimeSinceStartup - realTimeStart) < timeLimit && progressBar.value < 1f)
        {
            // Calcula el tiempo restante en tiempo real
            float elapsed = Time.realtimeSinceStartup - realTimeStart;
            currentTime = timeLimit - elapsed;
            timerText.text = "Tiempo: " + Mathf.Max(currentTime, 0f).ToString("F1") + "s";
            yield return null;
        }

        if (progressBar.value >= 1f)
        {
            CompletePuzzle();
        }
        else
        {
            FailPuzzle();
        }
    }

    void OnClickButton()
    {
        if (!isPuzzleActive) return;
        progressBar.value += progressPerClick;
    }

    void CompletePuzzle()
    {
        Debug.Log("✅ Puzzle completado!");

        // 🔹 Apagar el texto "Interactuar (I)"
        var interactScript = messyDesk.GetComponent<DeskInteract>();
        if (interactScript != null && interactScript.interactText != null)
            interactScript.interactText.gameObject.SetActive(false);

        // 🔹 Cambiar escritorios
        messyDesk.SetActive(false);
        organizedDesk.SetActive(true);

        puzzlePanel.SetActive(false);
        isPuzzleActive = false;

        // 🔹 Reanudar el juego
        Time.timeScale = 1f;

        // 🔹 Ocultar el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FailPuzzle()
    {
        Debug.Log("❌ Puzzle fallido...");
        puzzlePanel.SetActive(false);
        isPuzzleActive = false;

        // 🔹 Reanudar el juego
        Time.timeScale = 1f;

        // 🔹 Ocultar el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}