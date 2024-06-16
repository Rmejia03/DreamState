using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMapPortal : MonoBehaviour
{
    // Start is called before the first frame update
    public string mainMap;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(mainMap);
        }
    }
}
