using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class ForestBoss : MonoBehaviour, IDamage
{
    [Header("Enemy Info")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator animate;
    [SerializeField] Transform headPosition;
    [SerializeField] float HP;
    [SerializeField] int animateSpeedTransition;
    [SerializeField] int ViewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float attackRate;

    [Header("Melee Attack")]
    [SerializeField] bool isMelee;
    [SerializeField] float meleeRange;
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeAnimDur;

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
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        stoppingDistanceOrigin = agent.stoppingDistance;
        HPOrigin = HP;

        //player = gameManager.instance.player.GetComponent<playerControl>();
        //UpdateEnemyUI();

    }

    // Update is called once per frame
    void Update()
    {
        float animateSpeed = agent.velocity.normalized.magnitude;
        animate.SetFloat("Speed", Mathf.Lerp(animate.GetFloat("Speed"), animateSpeed, Time.deltaTime * animateSpeedTransition));
      
        if (playerInRange && !isAttacking && CanSeePlayer())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

            if (isMelee && distanceToPlayer <= meleeRange)
            {
                StartCoroutine(RandomAttack());
                
            }

            else
                agent.SetDestination(gameManager.instance.player.transform.position);
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

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FaceTarget();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            animate.SetTrigger("Rage");

            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
            player.fearVision.ResetFearCo = player.fearVision.StartCoroutine(player.fearVision.ResetFear());
        }
    }

    IEnumerator RandomAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            

            int attackChoice = Random.Range(1, 4);

            switch (attackChoice)
            {
                case 1:
                    animate.SetTrigger("Attack 1");
                    break;
                case 2:
                    animate.SetTrigger("Attack 2");
                    break;
                case 3:
                    animate.SetTrigger("Attack 3");
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(meleeAnimDur);
            isAttacking = false;
        }
    }


    public void meleeHit()
    {
        if (IsInRangeForMelee())
        {
            gameManager.instance.player.GetComponent<IDamage>().takeDamage(meleeDamage);
        }
    }
    

    public void takeDamage(float damage, bool slowFlash = false)
    {
        HP -= damage;

        //UpdateEnemyUI();

        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(hitFlash());

        if (HP <= 0)
        {
            //gameManager.instance.updateGameGoal(-1); 
            StartCoroutine(PlayDeathAnimation());

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

   

}
