using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBoss : MonoBehaviour
{
    [SerializeField] GameObject portal;

    private void BossDeath()
    {
        ActivatePortal();
    }

    private void ActivatePortal()
    {
        if(portal != null)
        {
            portal.SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            BossDeath();
        }
    }
}
