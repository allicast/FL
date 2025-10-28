using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemOptionsUI : MonoBehaviour
{
    public static ItemOptionsUI instance;

    [Header("Referencias del Panel")]
    public GameObject panel;

    [Header("Texto del título")]
    public TextMeshProUGUI itemTitle;

    [Header("Botones")]
    public Button useButton;
    public Button inspectButton;
    public Button dropButton;

    private InventoryItem currentItem;

    void Awake()
    {
        instance = this;
        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        if (panel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(),
                Input.mousePosition,
                null))
            {
                HideOptions();
            }
        }
    }

    public void ShowOptions(InventoryItem item, Vector3 position)
    {
        currentItem = item;

        if (itemTitle != null)
            itemTitle.text = item.name;

        if (panel != null)
        {
            panel.SetActive(true);
            panel.transform.position = position;
        }
    }

    public void HideOptions()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public bool IsOpenFor(InventoryItem item)
    {
        return panel.activeSelf && currentItem == item;
    }
    public void OnUse()
    {
        Debug.Log("Usando: " + currentItem.name);
        HideOptions();
    }

    public void OnInspect()
    {
        Debug.Log("Inspeccionando: " + currentItem.name);
        HideOptions();
    }

    public void OnDrop()
    {
        Debug.Log("Tirando: " + currentItem.name);
        InventoryManager.instance.items.Remove(currentItem);
        InventoryManager.instance.UpdateUI();
        HideOptions();
    }
}