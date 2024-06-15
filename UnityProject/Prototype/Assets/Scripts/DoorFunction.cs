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
    // Start is called before the first frame update
    void Start()
    {
        Instructions.SetActive(false);
        CloseTxt.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (doorOpen)
            {
                CloseTxt.SetActive(true);
            }
            else
            {
                Instructions.SetActive(true);
            }
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
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
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
            Action = false;
        }
    }
}
