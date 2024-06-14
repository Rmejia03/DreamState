using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

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

    private void Awake()
    {
        Instance = this;
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

    public void ListItems()
    {
        foreach( var item in inventory)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("Item/ItemIcon").GetComponent<Image>();

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

    //public bool IsHealingItemSelected()
    //{
    //    return inventory[selectedItem] != null && inventory[selectedItem].itemName == healingItem.itemName;
    //}


    //public bool IsFearItemSelected()
    //{
    //    return inventory[selectedItem] != null && inventory[selectedItem].itemName == fearItem.itemName;
    //}
}
