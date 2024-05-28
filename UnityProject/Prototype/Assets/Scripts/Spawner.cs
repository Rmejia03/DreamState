using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPosition;

    int spawnCount;
    bool isSpawn;
    bool startSpawn;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if(startSpawn && !isSpawn && spawnCount < numToSpawn)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            startSpawn = true;  
        }
    }

    IEnumerator spawn()
    {
        isSpawn = true;
        int arrayPosition = Random.Range(0, spawnPosition.Length);
        Instantiate(spawnObject, spawnPosition[arrayPosition].position, spawnPosition[arrayPosition].rotation);
        yield return new WaitForSeconds(spawnTimer);
        isSpawn = false;
    }
}
