using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    public enum Stage
    {
        waitingToStart,
        Stage1,
        Stage2,
        Stage3,
    }

    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject shield2;
    [SerializeField] private spawner SpawnEnemies;
    [SerializeField] private EnemyAI BossEnemy;

    private Stage stage;

    private void Start()
    {
        shield.SetActive(false);
        shield2.SetActive(false);
    }
    private void Awake()
    {
        stage = Stage.waitingToStart;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        StartNextStage();
        SpawnEnemy();
    }

    private void BossDead()
    {
        float health = GetBossHealth();

        if(health == 0)
        {
            DestroyAllEnemies();
        }
    }

    private void OnDamage()
    {
        float health = GetBossHealth();

        switch (stage)
        {
            case Stage.Stage1:
                if(health >= 70)
                {
                    StartNextStage();
                }
                break;
            case Stage.Stage2:
                if (health >= 50)
                {
                    StartNextStage();
                }
                break;
        }
    }

    private void StartNextStage()
    {
        switch(stage)
        {
            case Stage.waitingToStart:
                BossEnemy.SetBehaviorStage1();
                stage = Stage.Stage1;
                break;

            case Stage.Stage1:
                stage = Stage.Stage2;
                BossEnemy.SetBehaviorStage2();
                StartCoroutine(ActivateShield(shield, 5f));
                break;

            case Stage.Stage2:
                stage = Stage.Stage3;
                BossEnemy.SetBehaviorStage3();
                StartCoroutine(ActivateShield(shield2, 5f));
                break;
        }
    }
    private void SpawnEnemy()
    {
        spawner enemySpawn = Instantiate(SpawnEnemies);
        
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
    public float GetBossHealth()
    {
        if(BossEnemy != null)
        {
            return BossEnemy.GetHealth();
        }
        else
        {
            return -1f;
        }
    }

    private IEnumerator ActivateShield(GameObject shield, float time)
    {
        shield.SetActive(true);
        yield return new WaitForSeconds(time);  
        shield.SetActive(false);
    }

}
