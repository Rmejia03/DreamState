using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] int ID;

    private void OnTriggerEnter(Collider other)
    {
        itemStats selectedItem = gameManager.instance.playerScript.InventoryManager.GetSelectedItem();
        if (other.CompareTag("Player") && selectedItem != null && selectedItem.isKey == true && ID == selectedItem.keyID)
        {
            Destroy(gameObject);
            gameManager.instance.playerScript.InventoryManager.RemoveItem(selectedItem);
            //gameManager.instance.playerScript.changeItem();
        }
    }
}
