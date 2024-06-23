using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ForestBoss : MonoBehaviour, IDamage
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] Renderer model;
    Transform playerTransform;
    bool isWalking = false;

    [SerializeField] float bossHealth = 10f;
    [SerializeField] GameObject portal;

    float currentHealth;

    public void Start()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.isStopped = true;
        currentHealth = bossHealth;
        if(portal != null)
        {
            portal.SetActive(false);
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
        animator.SetTrigger("Die");
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

    public void StartWalking()
    {
        isWalking = true;
        navMesh.isStopped = false;
        navMesh.SetDestination(playerTransform.position);
        animator.SetBool("isWalking", true);
        Debug.Log("Walking towards player");
    }

    public void ActivatePortal()
    {
        if(portal != null)
        {
            portal.SetActive(true);
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartReact();
        }
        if(isWalking && playerTransform != null)
        {
            navMesh.SetDestination(playerTransform.position);
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
}
