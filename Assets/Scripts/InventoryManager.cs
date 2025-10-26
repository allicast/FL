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

    [HideInInspector]
    public List<InventoryItem> items = new List<InventoryItem>();

    void Awake()
    {
        // Singleton simple
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool newState = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(newState);

            // 🟢 Bloquear o desbloquear movimiento y cámara del jugador
            PlayerMove.isInventoryOpen = newState;

            // 🟢 Mostrar u ocultar el cursor
            if (newState)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void AddItem(Sprite image, string name)
    {
        InventoryItem newItem = new InventoryItem(name, image);
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
            {
                slotUI.Setup(item.image, item.name);
            }
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public string name;
    public Sprite image;

    public InventoryItem(string name, Sprite image)
    {
        this.name = name;
        this.image = image;
    }
}
