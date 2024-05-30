using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class inventoryManager : MonoBehaviour
{
    [Header("items")]
    [SerializeField] List<itemStats> inventory = new List<itemStats>();
    public int selectedItem;

    public itemStats healingItem;
    public itemStats shieldItem;

    public void AddItem(itemStats item)
    {
        inventory.Add(item);
        selectedItem = inventory.Count - 1;
    }

    public void RemoveItem(itemStats item)
    {
        if(inventory.Contains(item))
        {
            inventory.Remove(item);

            if(selectedItem >= inventory.Count)
            {
                selectedItem = inventory.Count - 1;
            }
        }
    }

    public itemStats GetSelectedItem()
    {
        if(inventory.Count > 0)
        {
            return inventory[selectedItem];
        }
        return null;
    }

    public void SelectNextItem()
    {
        if (selectedItem < inventory.Count - 1)
        {
            selectedItem++;
        }
    }

    public void SelectPreviousItem()
    {
        if(selectedItem > 0)
        {
            selectedItem--;
        }
    }

    public bool IsHealingItemSelected()
    {
        return inventory[selectedItem] != null && inventory[selectedItem].itemName == healingItem.itemName;
    }

    public bool IsShieldItemSelected()
    {
        return inventory[selectedItem] != null && inventory[selectedItem].itemName == shieldItem.itemName;
    }
}
