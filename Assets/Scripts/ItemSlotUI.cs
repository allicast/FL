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
            itemName.gameObject.SetActive(false); // Desactiva el texto
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
    }
}