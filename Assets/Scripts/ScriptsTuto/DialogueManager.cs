using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Sprite frameSprite;       // marco decorativo (izq o der)

    public CharacterSide side;
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI - Textos")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    [Header("UI - Personajes")]
    public Image leftCharacterImage;
    public Image rightCharacterImage;

    [Header("UI - Marcos decorativos")]
    public Image leftFrameImage;
    public Image rightFrameImage;

    [Header("Botón siguiente")]
    public Button nextButton;

    [Header("Configuración")]
    public float typeSpeed = 0.04f;
    public DialogueLine[] lines;

    private int index = 0;
    private bool isTyping = false;
    private bool skip = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        nextButton.onClick.AddListener(NextDialogue);
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

        nameText.text = line.characterName;

        // RESET: apagar todo antes de activar lo necesario
        leftCharacterImage.gameObject.SetActive(false);
        rightCharacterImage.gameObject.SetActive(false);
        leftFrameImage.gameObject.SetActive(false);
        rightFrameImage.gameObject.SetActive(false);

        // --- Mostrar en izquierda ---
        if (line.side == CharacterSide.Left)
        {
            // personaje
            leftCharacterImage.sprite = line.characterSprite;
            leftCharacterImage.gameObject.SetActive(true);

            // marco decorativo
            leftFrameImage.sprite = line.frameSprite;
            leftFrameImage.gameObject.SetActive(true);
        }
        else
        {
            // personaje
            rightCharacterImage.sprite = line.characterSprite;
            rightCharacterImage.gameObject.SetActive(true);

            // marco decorativo
            rightFrameImage.sprite = line.frameSprite;
            rightFrameImage.gameObject.SetActive(true);
        }

        // --- Efecto máquina de escribir ---
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
        }
    }
}