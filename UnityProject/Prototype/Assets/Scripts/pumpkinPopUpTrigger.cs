using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pumpkinPopUpTrigger : MonoBehaviour
{
    public enemyPopup pumpkinPopUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pumpkinPopUp != null)
            {
                pumpkinPopUp.StartPoppingUp();
            }
        }
    }

}

