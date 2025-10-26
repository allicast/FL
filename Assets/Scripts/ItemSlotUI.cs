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

    public void Setup(Sprite sprite, string name)
    {
        if (itemImage != null)
            itemImage.sprite = sprite;
    }

    public void OnClickSlot()
    {
        Debug.Log("Has seleccionado: " + itemName.text);
    }
}