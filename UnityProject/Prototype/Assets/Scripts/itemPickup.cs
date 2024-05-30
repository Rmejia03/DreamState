using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    [SerializeField] itemStats item;

    // Start is called before the first frame update
    void Start()
    {
        item.ammoCur = item.ammoMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.getItemStats(item);
            Destroy(gameObject);
        }
    }
}
