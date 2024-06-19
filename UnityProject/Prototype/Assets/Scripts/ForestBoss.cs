using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBoss : MonoBehaviour
{
    Animator animation;

    [SerializeField] GameObject portal;


    private void Start()
    {
        animation = GetComponent<Animator>();
    }
    private void BossDeath()
    {
        ActivatePortal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayNextAnimation();
        }
    }

    private void PlayNextAnimation()
    {
        if(animation != null)
        {
            animation.SetTrigger("DetectionTrigger");
        }
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
        
    }
}
