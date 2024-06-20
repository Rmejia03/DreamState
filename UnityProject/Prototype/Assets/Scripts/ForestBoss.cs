using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForestBoss : MonoBehaviour
{
    Animator animation;
    NavMeshAgent navMesh;
    Transform playerTransform;
    bool chase = false;

    [SerializeField] GameObject portal;


    private void Start()
    {
        animation = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        if(portal != null)
        {
            portal.SetActive(false);
        }

        navMesh.isStopped = true;
    }
    private void BossDeath()
    {
        ActivatePortal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            BossRage();
        }
    }

    private void BossRage()
    {
        if(animation != null)
        {
            animation.SetTrigger("DetectionTrigger");
        }
    }

    private void ChasePlayer()
    {
        chase = true;
        navMesh.isStopped = false;
    }

    private void ActivatePortal()
    {
        if(portal != null)
        {
            portal.SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            BossRage();
        }
        if(chase && playerTransform != null)
        {
            navMesh.SetDestination(playerTransform.position);
        }
    }
}
