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

    private GameObject shield;

    [SerializeField] private spawner SpawnEnemies;
    [SerializeField] private GameObject BossPrefab;
    [SerializeField] private Transform BossSpawnPos;

    private EnemyAI BossEnemy;
    private Stage stage;

    private void Start()
    {
        if(shield == null)
        {
            Debug.LogError("Shield is not assigned in the EnemyAI script.");
        }
        else
        {
            shield.SetActive(false);
        }
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
        SpawnBoss();
        StartNextStage();
    }

    public void BossDead()
    {
        float health = GetBossHealth();

        if(health <= 0)
        {
            DestroyAllEnemies();
        }
    }

    public void OnDamage()
    {
        float health = GetBossHealth();

        switch (stage)
        {
            case Stage.Stage1:
                if(health <= 70)
                {
                    Debug.Log("Transitioning to Stage 2");
                    StartNextStage();
                }
                break;
            case Stage.Stage2:
                if (health <= 50)
                {
                    Debug.Log("Transitioning to Stage 3");
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
                Debug.Log("Starting Stage 1");
                break;

            case Stage.Stage1:
                stage = Stage.Stage2;
                StartCoroutine(ActivateShield(shield, 5f));
                BossEnemy.SetBehaviorStage2();
                Debug.Log("Starting Stage 2");
                break;

            case Stage.Stage2:
                stage = Stage.Stage3;
                StartCoroutine(ActivateShield(shield, 5f));
                BossEnemy.SetBehaviorStage3();
                Debug.Log("Starting Stage 3");
                break;
        }
    }

    private void SpawnBoss()
    {
       GameObject bossInstance = Instantiate(BossPrefab, BossSpawnPos.position, BossSpawnPos.rotation);
        BossEnemy = bossInstance.GetComponent<EnemyAI>();

        shield = BossEnemy.shield;

        //BossEnemy.bossBattle = this;
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

    public void AssignShield(GameObject shield)
    {
        this.shield = shield;
    }

    private IEnumerator ActivateShield(GameObject shield, float time)
    {
        Debug.Log("Activating shield: " + shield.name);
        shield.SetActive(true);
        yield return new WaitForSeconds(time);  
        shield.SetActive(false);
        Debug.Log("Deactivating shield: " + shield.name);
    }
}
