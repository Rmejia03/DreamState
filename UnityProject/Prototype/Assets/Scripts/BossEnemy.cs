using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour, IDamage
{
    public enum Stage
    {
        waitingToStart,
        Stage1,
        Stage2,
        Stage3,
    }

    //[SerializeField] private spawner SpawnEnemies;
    [Header("Enemy Info")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator animate;
    [SerializeField] Transform headPosition;
    [SerializeField] float HP;
    public GameObject shield;
    [SerializeField] int animateSpeedTransition;
    [SerializeField] int ViewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float attackRate;

    [Header("Range Attack")]
    [SerializeField] Transform shootPOS;
    [SerializeField] GameObject bullet;
    [SerializeField] int shootRange;
    [SerializeField] float shootStopDis;

    [Header("Melee Attack")]
    [SerializeField] bool isMelee;
    [SerializeField] float meleeRange;
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeAnimDur;

    [Header("Roam")]
    [SerializeField] int roamDistance;
    [SerializeField] int roamTimer;

    [Header("Death Animation")]
    [SerializeField] AnimationClip deathAnimation;
    [SerializeField] float deathAniDuration;

    bool isAttacking;
    bool playerInRange;
    bool destinationChosen;
    float angleToPlayer;
    float stoppingDistanceOrigin;

    Vector3 startingPosition;
    Vector3 playerDirection;

    float HPOrigin;

    public playerControl player;
    private Stage stage;
    private bool shieldActivated;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        stoppingDistanceOrigin = agent.stoppingDistance;
        HPOrigin = HP;

        if (shield == null)
        {
            shield.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float animateSpeed = agent.velocity.normalized.magnitude;
        animate.SetFloat("Speed", Mathf.Lerp(animate.GetFloat("Speed"), animateSpeed, Time.deltaTime * animateSpeedTransition));

        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

        if (!playerInRange || !CanSeePlayer())
        {
            StartCoroutine(Roam());
        }

        if (playerInRange && !isAttacking && CanSeePlayer())
        {
            if (isMelee && distanceToPlayer <= meleeRange)
            {
                StartCoroutine(MeleeAttack());
            }
            else if (!isMelee && distanceToPlayer <= shootRange)
            {
                agent.stoppingDistance = shootStopDis;
                StartCoroutine(Shoot());
            }
            else
            {
                agent.stoppingDistance = stoppingDistanceOrigin;
                agent.SetDestination(gameManager.instance.player.transform.position);
            }
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
            playerInRange = true;
            StartBattle();
        }
    }

    private void StartBattle()
    {
        StartNextStage();
    }

    //Roaming Enemy
    IEnumerator Roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05f)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;

            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPosition = Random.insideUnitSphere * roamDistance;
            randomPosition += startingPosition;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    //See player within range
    bool CanSeePlayer()
    {
        playerDirection = gameManager.instance.player.transform.position - headPosition.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, playerDirection.y + 1, playerDirection.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPosition.position, playerDirection);

        RaycastHit hit;

        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= ViewAngle)
            {
                agent.stoppingDistance = stoppingDistanceOrigin;
                agent.SetDestination(gameManager.instance.player.transform.position);

                float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

                if (stage == Stage.Stage2 || stage == Stage.Stage3)
                {
                    agent.stoppingDistance = shootStopDis;

                    if (distanceToPlayer <= shootRange)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                else if (isMelee && !isAttacking && distanceToPlayer <= meleeRange)
                {
                    StartCoroutine(MeleeAttack());
                }
                else if (!isMelee && !isAttacking)
                {
                    StartCoroutine(Shoot());
                }

                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    //Check if in melee range
    bool IsInRangeForMelee()
    {
        return Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= meleeRange;
    }

    //Rotation To Face Player
    void FaceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceTargetSpeed);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 5;
            player.fearVision.ResetFearCo = player.fearVision.StartCoroutine(player.fearVision.ResetFear());
        }
    }

    IEnumerator Shoot()
    {
        isAttacking = true;
        animate.SetTrigger("Attack");

        float ogStopDis = agent.stoppingDistance;
        agent.stoppingDistance = shootStopDis;

        createBullet();

        yield return new WaitForSeconds(attackRate);

        agent.stoppingDistance = ogStopDis;

        isAttacking = false;
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPOS.position, shootPOS.rotation);
    }

    public void meleeHit()
    {
        if (IsInRangeForMelee())
        {
            gameManager.instance.player.GetComponent<IDamage>().takeDamage(meleeDamage);
        }
    }
    IEnumerator MeleeAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animate.SetTrigger("MAttack");

            yield return new WaitForSeconds(meleeAnimDur);
            //meleeHit();
            isAttacking = false;
        }
    }

    public void takeDamage(float damage, bool slowFlash = false)
    {
        if(shield.activeSelf)
        {
            return;
        }

        HP -= damage;
        OnDamage();
        //UpdateEnemyUI();

        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(hitFlash());

        if (HP <= 0)
        {
            StartCoroutine(PlayDeathAnimation());
            DestroyAllEnemies();
        }
    }

    public void OnDamage()
    {
        switch (stage)
        {
            case Stage.Stage1:
                if (HP <= 70)
                {
                    Debug.Log("Transitioning to Stage 2");
                    StartNextStage();
                }
                break;
            case Stage.Stage2:
                if (HP <= 50)
                {
                    Debug.Log("Transitioning to Stage 3");
                    StartNextStage();
                }
                break;
        }
    }

    private void StartNextStage()
    {
        switch (stage)
        {
            case Stage.waitingToStart:
                stage = Stage.Stage1;
                Debug.Log("Starting Stage 1");
                break;

            case Stage.Stage1:
                stage = Stage.Stage2;
                isMelee = false;
                StartCoroutine(ActivateShield(shield, 5f));
                Debug.Log("Starting Stage 2");
                break;

            case Stage.Stage2:
                shieldActivated = false;    
                stage = Stage.Stage3;
                StartCoroutine(ActivateShield(shield, 5f));
                Debug.Log("Starting Stage 3");
                break;
        }
    }

    IEnumerator PlayDeathAnimation()
    {
        animate.SetTrigger("Death");
        yield return new WaitForSeconds(deathAniDuration);

        Destroy(gameObject);
    }

    IEnumerator hitFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    //Enemy HP
    void UpdateEnemyUI()
    {
        gameManager.instance.enemyHPBar.fillAmount = (float)HP / HPOrigin;
    }
    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
    private IEnumerator ActivateShield(GameObject shield, float time)
    {
        if(shieldActivated)
        {
            yield break;
        }

        shieldActivated = true;

        Debug.Log("Activating shield: " + shield.name);
        shield.SetActive(true);
        yield return new WaitForSeconds(time);
        shield.SetActive(false);
        Debug.Log("Deactivating shield: " + shield.name);
    }
}
