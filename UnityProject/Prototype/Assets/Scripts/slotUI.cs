using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class slotUI : MonoBehaviour
{
    public Image itemIcon;

    [SerializeField] private GameObject highlight;

    public void UpdateSlot(Sprite newSprite)
    {
        //Debug.Log($"UpdateSlot called with newsprite");

        //if(itemIcon == null)
        //{
        //    Debug.LogError("Item icon is not assigned in the slotUI component");
        //    return;
        //}

        itemIcon.sprite = newSprite;
        itemIcon.enabled = newSprite != null;
    }
}
