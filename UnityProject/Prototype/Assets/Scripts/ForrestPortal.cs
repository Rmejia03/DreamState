using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestPortal : MonoBehaviour
{
    public string forrestScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inventoryManager.Instance.SaveInventory();

            SceneManager.LoadScene(forrestScene);
        }
    }
}
