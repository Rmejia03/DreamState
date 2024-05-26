using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] Renderer model;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameManager.instance.playerSpawnPos.transform.position != transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(checkPopup());
        }
    }

    IEnumerator checkPopup()
    {
        model.material.color = Color.green;
        gameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        gameManager.instance.checkpointPopup.SetActive(false);
        model.material.color = Color.white;
    }
}
