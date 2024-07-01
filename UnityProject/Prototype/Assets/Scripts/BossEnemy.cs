using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] string bossName;

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

    [Header("Health Bar")]
    [SerializeField] Image healthBarFill;
    [SerializeField] TMP_Text healthBarNameText;
    [SerializeField] GameObject healthBarUI;

	[Header("Audio")]
	public AudioSource bossAudio;
	public AudioClip bossPain;

	[Header("portal")]
    [SerializeField] GameObject portal;

    [Header("Poison Bubble")]
    [SerializeField] GameObject poisonPrefab;
    [SerializeField] float poisonDuration = 5f;
    [SerializeField] float poisonDamage = .5f;
    [SerializeField] float poisonTickRate = 1f;
    public GameObject poisonInstance;

    bool isAttacking;
    bool playerInRange;
    bool destinationChosen;
    float angleToPlayer;
    float stoppingDistanceOrigin;
    bool battleStarted = false;
    bool poisonActive = false;
    bool isApplyingPoisonDmg = false;

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
		bossAudio = GetComponent<AudioSource>();

		if (healthBarUI  != null)
        {
            healthBarUI.SetActive(false);
        }

        if (shield == null)
        {
            shield.SetActive(false);
        }


        if (poisonPrefab != null)
        {
            poisonInstance = Instantiate(poisonPrefab, transform);
            poisonInstance.SetActive(false);
            //Debug.Log("Poison bubble instantiated and set inactive.");
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
            FaceTarget();

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
            
            if(!battleStarted)
            {
                StartBattle();
            }

            if(stage == Stage.Stage3 && !isApplyingPoisonDmg)
            {
                StartCoroutine(ApplyPoisonDmg(other));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && stage == Stage.Stage3 && poisonActive && !isApplyingPoisonDmg)
        {
            StartCoroutine(ApplyPoisonDmg(other));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 5;
            //player.fearVision.ResetFearCo = player.fearVision.StartCoroutine(player.fearVision.ResetFear());

            if (stage == Stage.Stage3)
            {
                isApplyingPoisonDmg = false;
                StopCoroutine(ApplyPoisonDmg(other));
            }
        }
    }
    private void StartBattle()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
            healthBarNameText.text = bossName;
            UpdateEnemyUI();
        }
        battleStarted = true;
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

    IEnumerator Shoot()
    {
        isAttacking = true;
        animate.SetTrigger("Attack");

        float ogStopDis = agent.stoppingDistance;
        agent.stoppingDistance = shootStopDis;
        FaceTarget();
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
            meleeHit();
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
        UpdateEnemyUI();
		bossAudio.PlayOneShot(bossPain);

		agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(hitFlash());

        if (HP <= 0)
        {
            StartCoroutine(PlayDeathAnimation());
            DestroyAllEnemies();
            healthBarUI.SetActive(false);
            portal.SetActive(true);
            BossManager.instance.BossDefeated(1);
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
                //Debug.Log("Starting Stage 1");
                break;

            case Stage.Stage1:
                stage = Stage.Stage2;
                isMelee = false;
                StartCoroutine(ActivateShield(shield, 5f));
                Shoot();
                //Debug.Log("Starting Stage 2");
                break;

            case Stage.Stage2:
                stage = Stage.Stage3;
                shieldActivated = false;
                StartCoroutine(ActivateShield(shield, 5f));
                isMelee = true;
                meleeAnimDur /= 2;
                meleeDamage *= 2;
                //Debug.Log("Starting Stage 3");

                if (poisonInstance != null)
                {
                    poisonInstance.SetActive(true);
                    poisonActive = true;
                    StartCoroutine(DeactivatePoison(poisonInstance, poisonDuration));
                }
                break;
        }
    }

    IEnumerator PlayDeathAnimation()
    {
        animate.SetTrigger("Death");
        isAttacking = false;
        faceTargetSpeed = 0;
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
        if(healthBarFill != null)
        {
            healthBarFill.fillAmount = HP / HPOrigin;
        }
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

        //Debug.Log("Activating shield: " + shield.name);
        shield.SetActive(true);
        yield return new WaitForSeconds(time);
        shield.SetActive(false);
        //Debug.Log("Deactivating shield: " + shield.name);
    }

    IEnumerator ApplyPoisonDmg(Collider player)
    {
        float time = 0f;
        isApplyingPoisonDmg = true;
        while (time < poisonDuration && isApplyingPoisonDmg)
        {

            if(player.CompareTag("Player"))
            {
                player.GetComponent<IDamage>().takeDamage(poisonDamage);
            }
          
            time += poisonTickRate;
            yield return new WaitForSeconds(poisonTickRate);
        }
        isApplyingPoisonDmg = false;
    }

    IEnumerator DeactivatePoison(GameObject poison, float time)
    {
        yield return new WaitForSeconds(time);
        poison.SetActive(false);
        poisonActive = false;
        isApplyingPoisonDmg = false;
    }
}
