using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform spawnPoint;
    public Transform player;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasSpawned)
        {
            SpawnBoss();
            hasSpawned = true;
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        ForestBoss forestBoss = boss.GetComponent<ForestBoss>();

        if(forestBoss != null)
        {
            forestBoss.playerTransform = player;
            Debug.Log("Boss spawned, player assigned");
        }
        
            

    }
}
