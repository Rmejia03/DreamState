using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckpoint : MonoBehaviour
{
    public TutorialManager tutorialManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.ShowNextStep();
            Destroy(gameObject);
        }
    }
}
