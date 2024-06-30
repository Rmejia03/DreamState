using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalMainToForest : MonoBehaviour
{
    public string mainToForest;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManager.Instance.SaveInventory();

            SceneManager.LoadScene(mainToForest);
            Destroy(gameObject);
        }
    }
}
