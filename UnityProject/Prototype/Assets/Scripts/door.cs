using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    [SerializeField] Renderer model;

    private void OnTriggerEnter(Collider other)
    {
        itemStats selectedItem = gameManager.instance.playerScript.inventoryManager.GetSelectedItem();
        if (other.CompareTag("Player") && selectedItem != null && selectedItem.isKey == true)
        {
            Destroy(gameObject);
            gameManager.instance.playerScript.inventoryManager.RemoveItem(selectedItem);
            //gameManager.instance.playerScript.changeItem();
        }
    }
}
