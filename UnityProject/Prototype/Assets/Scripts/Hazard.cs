using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private bool isMushroom = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerControl playerHP = other.GetComponent<playerControl>();
            if (playerHP != null)
            {
                if (isMushroom)
                {
                    playerHP.takeDamage(damagePerSecond * Time.deltaTime, true);

                }
                else
                {
                    playerHP.takeDamage(damagePerSecond * Time.deltaTime);
                }
            }
        }
    }
}
