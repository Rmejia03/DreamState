using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorFunction : MonoBehaviour
{
    public GameObject Instructions;
    public GameObject CloseTxt;
    public GameObject Animation;
    public GameObject Trigger;
    public AudioSource OpenSound;
    public AudioSource CloseSound;
    public bool Action = false;
    public bool doorOpen = false;
    public float interactDistance = 3f;

    private Transform playerTransform;
    private bool isAnimating = false;

    // Start is called before the first frame update
    void Start()
    {
        Instructions.SetActive(false);
        CloseTxt.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            InstructionText();
            Action = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instructions.SetActive(false);
            CloseTxt.SetActive(false);
            Action = false;
            playerTransform = null;
        }
    }

    private void InstructionText()
    {
        if(doorOpen)
        {
            CloseTxt.SetActive(true);
            Instructions.SetActive(false);
        }
        else
        {
            Instructions.SetActive(true);
            CloseTxt.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null && Action && Vector3.Distance(playerTransform.position, transform.position) <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
            {
                StartCoroutine(DoorInteraction());    
            }
        }
    }

    private IEnumerator DoorInteraction()
    {
        isAnimating = true;
        if (!doorOpen)
        {
            Trigger.SetActive(true);
            Instructions.SetActive(true);
            Animation.GetComponent<Animator>().Play("DoorOpen");
            OpenSound.Play();
            doorOpen = true;
            Instructions.SetActive(false);
            CloseTxt.SetActive(true);
        }
        else
        {
            Trigger.SetActive(true);
            CloseTxt.SetActive(true);
            Animation.GetComponent<Animator>().Play("DoorClose");
            CloseSound.Play();
            doorOpen = false;
            CloseTxt.SetActive(false);
            Instructions.SetActive(true);
        }
        InstructionText();
        yield return new WaitForSeconds(Animation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        isAnimating = false;
    }
}
