using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum CharacterSide
{
    Left,
    Right
}

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(2, 5)]
    public string text;

    public Sprite characterSprite;   // retrato del personaje
    public Sprite frameSprite;       // marco decorativo

    public CharacterSide side;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Panel del diálogo")]
    public GameObject dialoguePanel;   // <<--- AGREGADO

    [Header("UI - Textos")]
    public TextMeshProUGUI leftNameText;
    public TextMeshProUGUI rightNameText;
    public TextMeshProUGUI dialogText;

    [Header("UI - Personajes")]
    public Image leftCharacterImage;
    public Image rightCharacterImage;

    [Header("UI - Marcos decorativos")]
    public Image leftFrameImage;
    public Image rightFrameImage;

    [Header("Botones siguiente (izquierda y derecha)")]
    public Button nextButtonLeft;
    public Button nextButtonRight;

    [Header("Configuración")]
    public float typeSpeed = 0.04f;
    public DialogueLine[] lines;

    private int index = 0;
    private bool isTyping = false;
    private bool skip = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        // ACTIVAR PANEL AL INICIAR LA ESCENA
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);   // <<--- AGREGADO

        // Ambos botones hacen lo mismo
        if (nextButtonLeft != null)
            nextButtonLeft.onClick.AddListener(NextDialogue);

        if (nextButtonRight != null)
            nextButtonRight.onClick.AddListener(NextDialogue);

        ShowLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            skip = true;
    }

    void ShowLine()
    {
        DialogueLine line = lines[index];

        // APAGAR todo
        leftCharacterImage.gameObject.SetActive(false);
        rightCharacterImage.gameObject.SetActive(false);
        leftFrameImage.gameObject.SetActive(false);
        rightFrameImage.gameObject.SetActive(false);
        leftNameText.gameObject.SetActive(false);
        rightNameText.gameObject.SetActive(false);

        // IZQUIERDA
        if (line.side == CharacterSide.Left)
        {
            leftCharacterImage.sprite = line.characterSprite;
            leftCharacterImage.gameObject.SetActive(true);

            leftFrameImage.sprite = line.frameSprite;
            leftFrameImage.gameObject.SetActive(true);

            leftNameText.text = line.characterName;
            leftNameText.gameObject.SetActive(true);
        }
        else
        {
            // DERECHA
            rightCharacterImage.sprite = line.characterSprite;
            rightCharacterImage.gameObject.SetActive(true);

            rightFrameImage.sprite = line.frameSprite;
            rightFrameImage.gameObject.SetActive(true);

            rightNameText.text = line.characterName;
            rightNameText.gameObject.SetActive(true);
        }

        // EFECTO MÁQUINA DE ESCRIBIR
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        skip = false;
        isTyping = true;

        foreach (char c in text)
        {
            if (skip)
            {
                dialogText.text = text;
                break;
            }

            dialogText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    public void NextDialogue()
    {
        if (isTyping)
        {
            skip = true;
            return;
        }

        index++;

        if (index < lines.Length)
        {
            ShowLine();
        }
        else
        {
            Debug.Log("Fin del diálogo");

            SceneManager.LoadScene("World");

            //if (dialoguePanel != null)//
            //dialoguePanel.SetActive(false);//
        }
    }
}