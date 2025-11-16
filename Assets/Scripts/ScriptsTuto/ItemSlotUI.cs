using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image itemImage;
    public TextMeshProUGUI itemName;

    private void Start()
    {
        if (itemName != null)
            itemName.gameObject.SetActive(false);
    }

    public void Setup(Sprite sprite, string name)
    {
        if (itemImage != null)
            itemImage.sprite = sprite;
        if (itemName != null)
            itemName.text = name;
    }

    public void OnClickSlot()
    {
        Debug.Log("Has seleccionado: " + itemName.text);

        if (itemName == null) return;

        InventoryItem clickedItem = InventoryManager.instance.items.Find(i => i.name == itemName.text);

        if (clickedItem != null && ItemOptionsUI.instance != null)
        {
            if (ItemOptionsUI.instance.IsOpenFor(clickedItem))
            {
                ItemOptionsUI.instance.HideOptions();
                return;
            }
            ItemOptionsUI.instance.HideOptions();

            Vector3 slotPosition = transform.position;

            ItemOptionsUI.instance.ShowOptions(clickedItem, slotPosition + new Vector3(120f, 0f, 0f));
        }
    }
}