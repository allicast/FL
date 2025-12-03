using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Referencias del Inventario")]
    public GameObject inventoryPanel;
    public Transform itemsParent;
    public GameObject itemSlotPrefab;

    [Header("UI Adicional")]
    public GameObject crosshair;

    [HideInInspector]
    public List<InventoryItem> items = new List<InventoryItem>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (crosshair != null)
            crosshair.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool newState = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(newState);
            PlayerMove.isInventoryOpen = newState;

            if (newState)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (crosshair != null)
                    crosshair.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (crosshair != null)
                    crosshair.SetActive(true);
            }
        }
    }

    public void AddItem(Sprite image, string name, string useText)
    {
        InventoryItem newItem = new InventoryItem(name, image, useText);
        items.Add(newItem);
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemsParent);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            if (slotUI != null)
                slotUI.Setup(item.image, item.name);
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public string name;
    public Sprite image;
    [TextArea] public string useText;

    public InventoryItem(string name, Sprite image, string useText)
    {
        this.name = name;
        this.image = image;
        this.useText = useText;
    }
}