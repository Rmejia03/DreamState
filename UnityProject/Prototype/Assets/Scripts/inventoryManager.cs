using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor.UIElements;

public class inventoryManager : MonoBehaviour
{ 
    public static inventoryManager Instance;

    [Header("items")]
    public List<itemStats> inventory = new List<itemStats>();
    public int selectedItem;

    public Transform ItemContent;
    public GameObject InventoryItem;

    public itemStats healingItem;
    public int healingItemIndex;
    [SerializeField] TMP_Text healingPotionCount;

    public itemStats fearItem;
    public int fearItemIndex;
    [SerializeField] TMP_Text fearPotionCount;

    public itemStats key01Item;
    public ParticleSystem hitEffect;

    public toolbarUI ToolBarUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckScrollWheel();
        CheckAlphaNumericKey();
    }

    public void AddItem(itemStats item)
    {
        if (item.itemName == healingItem.itemName)
        {
            healingItemIndex += 1;
            healingPotionCount.SetText(healingItemIndex.ToString());
        }
        else if (item.itemName == fearItem.itemName)
        {
            fearItemIndex += 1;
            fearPotionCount.SetText(fearItemIndex.ToString());
        }
        else
        {
            inventory.Add(item);
            selectedItem = inventory.Count - 1;

            ToolBarUI.UpdateToolbar(inventory);
        }
    }
 
    public void RemoveItem(itemStats item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);

            if (selectedItem >= inventory.Count)
            {
                selectedItem = inventory.Count - 1;
            }
        }
    }

    private void CheckScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll > 0f)
        {
            SelectNextItem();
        }
        else if(scroll < 0f)
        {
            SelectPreviousItem();
        }
    }
    private void CheckAlphaNumericKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }
    }

    public void SelectSlot(int index)
    {
        if (index < inventory.Count)
        {
            selectedItem = index;
            ToolBarUI.SelectSlot(selectedItem);
        }
    }

    public itemStats GetSelectedItem()
    {
        if (inventory.Count > 0)
        {

            return inventory[selectedItem];
        }
        return null;
    }

    public void SelectNextItem()
    {
        selectedItem = (selectedItem + 1) % inventory.Count;
        ToolBarUI.SelectSlot(selectedItem);
    }

    public void SelectPreviousItem()
    {
        selectedItem = (selectedItem - 1 + inventory.Count) % inventory.Count;
        ToolBarUI.SelectSlot(selectedItem);
    }

    //public bool IsHealingItemSelected()
    //{
    //    return inventory[selectedItem] != null && inventory[selectedItem].itemName == healingItem.itemName;
    //}


    //public bool IsFearItemSelected()
    //{
    //    return inventory[selectedItem] != null && inventory[selectedItem].itemName == fearItem.itemName;
    //}
}
