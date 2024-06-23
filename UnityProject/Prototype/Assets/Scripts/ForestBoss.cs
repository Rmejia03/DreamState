using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ForestBoss : MonoBehaviour, IDamage
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] Renderer model;
    [SerializeField] Transform headPosition;
    [SerializeField] int animateSpeedTransition;
    [SerializeField] int ViewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] bool isMelee;
    [SerializeField] float meleeRange;
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeAnimDur;
    public Transform playerTransform;
    bool isWalking = false;

    
    [SerializeField] float bossHealth;
    [SerializeField] GameObject portal;

    float currentHealth;
    bool isAttacking;
    bool playerInRange;
    bool destinationChosen;
    float angleToPlayer;
    float stoppingDistanceOrigin;
   

    Vector3 startingPosition;
    Vector3 playerDirection;

    public void Start()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.isStopped = true;
        startingPosition = transform.position;
        stoppingDistanceOrigin = navMesh.stoppingDistance;
        currentHealth = bossHealth;
        if(portal != null)
        {
            portal.SetActive(false);
        }
        if(playerTransform != null)
        {
            Debug.LogError("Player Transform not assigned to boss");
        }
    }

    public void Update()
    {
        float animateSpeed = navMesh.velocity.normalized.magnitude;
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), animateSpeed, Time.deltaTime * animateSpeedTransition));

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartReact();
        }
        //if (isWalking && playerTransform != null)
        //{
        //    navMesh.SetDestination(playerTransform.position);
        //}

        if (playerInRange && !isAttacking && CanSeePlayer())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

            if (isMelee && distanceToPlayer <= meleeRange)
            {
                StartCoroutine(MeleeAttack());
            }
        }
    }
    //public void TakeDamage(float amount, bool slowFlash = false)
    //{
    //    currentHealth -= amount;
    //    if(currentHealth <= 0)
    //    {
    //        Die();
    //    }
    //}

    void Die()
    {
        animator.SetTrigger("Death");
        navMesh.isStopped = true;
        ActivatePortal();
        Destroy(gameObject, 2f);
    }
    public void BossDeath()
    {
        ActivatePortal();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            StartReact();
        }
    }

    public void StartReact()
    {
        animator.SetTrigger("TriggerReact");
    }

    //public void StartWalking()
    //{
    //    isWalking = true;
    //    navMesh.isStopped = false;
    //    navMesh.SetDestination(playerTransform.position);
    //    animator.SetBool("isWalking", true);
    //    Debug.Log("Walking towards player");
    //}

    public void ActivatePortal()
    {
        if(portal != null)
        {
            portal.SetActive(true);
        }
    }


    public void takeDamage(float amount, bool slowFlash = false)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

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
                navMesh.stoppingDistance = stoppingDistanceOrigin;
                navMesh.SetDestination(gameManager.instance.player.transform.position);

                float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

                if (navMesh.remainingDistance <= navMesh.stoppingDistance)
                {
                    FaceTarget();
                }

                if (isMelee && !isAttacking && distanceToPlayer <= meleeRange)
                {
                    StartCoroutine(MeleeAttack());
                }

                

                return true;
            }
        }
        navMesh.stoppingDistance = 0;
        return false;
    }

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
            animator.SetTrigger("MAttack");

            yield return new WaitForSeconds(meleeAnimDur);
            //meleeHit();
            isAttacking = false;
        }
    }


}
