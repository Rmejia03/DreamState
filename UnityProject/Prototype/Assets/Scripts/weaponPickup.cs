using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon.ammoCur = weapon.ammoMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.getWeaponStats(weapon);
            Destroy(gameObject);
        }
    }
}