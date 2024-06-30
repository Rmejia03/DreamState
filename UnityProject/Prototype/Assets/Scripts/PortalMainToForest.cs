using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalMainToForest : MonoBehaviour
{
    public string mainToForest;
    public string portalDestroyKey;

    private void Start()
    {
        if (PlayerPrefs.GetInt(portalDestroyKey, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManager.Instance.SaveInventory();

            PlayerPrefs.SetInt(portalDestroyKey, 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene(mainToForest);
        }
    }
}
