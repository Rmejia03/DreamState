using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMapPortal : MonoBehaviour
{
    public string mainMap;
    public string portalDestroyKey;

    private void Start()
    {
        int isPortalDestroyed = PlayerPrefs.GetInt(portalDestroyKey, 0);
        Debug.Log("Portal Destroyed Key: " + isPortalDestroyed);

        if (isPortalDestroyed == 1)
        {
            Destroy(gameObject);
        }
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

            SceneManager.LoadScene(mainMap);
            //Timer.Instance.stopTimer();
        }
    }
}
