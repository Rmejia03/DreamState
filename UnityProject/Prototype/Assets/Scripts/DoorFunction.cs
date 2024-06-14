using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorFunction : MonoBehaviour
{
    public GameObject Instructions;
    public GameObject Animation;
    //public GameObject Trigger;
    public AudioSource OpenSound;
    public AudioSource CloseSound;
    public bool Action = false;
    // Start is called before the first frame update
    void Start()
    {
        Instructions.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Instructions.SetActive(true);
            Action = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Instructions.SetActive(false);
        Action = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(Action == true)
            {
                Instructions.SetActive(false);
                Animation.GetComponent<Animator>().Play("DoorOpen");
                Animation.GetComponent<Animator>().Play("DoorClose");
                //Trigger.SetActive(false);
                OpenSound.Play();
                CloseSound.Play();
                Action = false;
            }
        }
    }
}
