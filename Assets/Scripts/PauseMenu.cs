using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel; 
    public Button resumeButton;   
    public Button quitButton; 

    bool isPaused = false;
    CursorLockMode previousLockState;
    bool previousCursorVisible;

    void Start()
    {
        
        if (pausePanel != null) pausePanel.SetActive(false);

        
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (quitButton != null) quitButton.onClick.AddListener(QuitToMainMenu);

        
        previousLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        
        if (pausePanel != null) pausePanel.SetActive(true);

        
        Time.timeScale = 0f;

        

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        
        if (pausePanel != null) pausePanel.SetActive(false);

        
        Time.timeScale = 1f;

        
        AudioListener.pause = false;

        
        Cursor.lockState = previousLockState;
        Cursor.visible = previousCursorVisible;
    }

    public void QuitToMainMenu()
    {
        
        Time.timeScale = 1f;
        AudioListener.pause = false;

        
        SceneManager.LoadScene("UI");
    }

  
}

