using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    public itemStats item;

    // Start is called before the first frame update
   // void Start()
    //
       // item.ammoCur = item.ammoMax;
   // }

    void Pickup()
    {
        inventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Pickup();
        }
    }
}
