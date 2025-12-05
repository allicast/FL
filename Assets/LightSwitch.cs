using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : BaseInteractable
{
    [SerializeField] GameObject lightObj;
    public override void Interact()
    {
        lightObj.SetActive(!lightObj.activeSelf);
    }
}
