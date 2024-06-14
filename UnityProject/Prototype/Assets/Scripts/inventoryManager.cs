using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class inventoryManager : MonoBehaviour
{ 
    public static inventoryManager Instance;

    [Header("items")]
    public List<itemStats> inventory = new List<itemStats>();
    public int selectedItem;

    public Transform ItemContent;
    public GameObject InventoryItem;

    public itemStats healingItem;
    public itemStats shieldItem;
    public itemStats key01Item;
    public ParticleSystem hitEffect;

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(itemStats item)
    {
        inventory.Add(item);
        //selectedItem = inventory.Count - 1;      
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

    public void ListItems()
    {
        foreach( var item in inventory)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemIcon.sprite = item.icon;
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
        if (selectedItem < inventory.Count - 1)
        {
            selectedItem++;
        }
    }

    public void SelectPreviousItem()
    {
        if (selectedItem > 0)
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
