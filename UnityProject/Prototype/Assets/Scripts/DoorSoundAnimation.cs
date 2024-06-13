using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DoorSoundAnimation : MonoBehaviour
{
    private Animator doorAnimation;
    private bool doorOpen = false;

    [SerializeField] private string OpenDoor;
    [SerializeField] private string CloseDoor;

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;

    [SerializeField] private AudioSource doorOpenSound;
    [SerializeField] private float openDelay = 0;
    [SerializeField] private AudioSource closeDoorSound;
    [SerializeField] private float closeDelay = 0;
    private float doorOpenTime;
    private void Awake()
    {
        doorAnimation = GetComponent<Animator>();
        doorOpenSound = GetComponent<AudioSource>();
        doorOpenTime = doorAnimation.GetCurrentAnimatorStateInfo(0).length + openDelay;
        //doorAnimation = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        Debug.Log("PlayAnimation called");
        if (!doorOpen && !pauseInteraction)
        {
            Debug.Log("Opening door");
            doorAnimation.Play(OpenDoor, 0, 0.0f);
            doorOpen = true;
            StartCoroutine(PauseDoor());
            doorOpenSound.PlayDelayed(openDelay);
        }
        else if (doorOpen && !pauseInteraction)
        {
            Debug.Log("Closing door");
            doorAnimation.Play(CloseDoor, 0, 0.0f);
            doorOpen = false;
            StartCoroutine(PauseDoor());
            closeDoorSound.PlayDelayed(closeDelay);
        }
    }  

    private IEnumerator PauseDoor()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }
}
