using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pumpkinPopUpTrigger : MonoBehaviour
{
    public pumpkinPopUpTrigger pumpkinPopUp;

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

