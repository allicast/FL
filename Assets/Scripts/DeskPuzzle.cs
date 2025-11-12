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
    public float timeLimit = 5f; // tiempo total para completar
    public float progressPerClick = 0.1f; // cuánto llena por click

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
        // Solo abrir el puzzle si el jugador presiona I y está cerca
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

        // 🔹 Mostrar y desbloquear el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(PuzzleTimer());
    }

    IEnumerator PuzzleTimer()
    {
        while (currentTime > 0f && progressBar.value < 1f)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Tiempo: " + currentTime.ToString("F1") + "s";
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
        puzzlePanel.SetActive(false);
        messyDesk.SetActive(false);
        organizedDesk.SetActive(true);
        isPuzzleActive = false;

        // 🔹 Ocultar y bloquear el cursor otra vez
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FailPuzzle()
    {
        Debug.Log("❌ Puzzle fallido...");
        puzzlePanel.SetActive(false);
        isPuzzleActive = false;

        // 🔹 También volver a ocultar y bloquear el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}