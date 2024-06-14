using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour
{
    public float interactionRange;
    public GameObject interactText;
    public string doorOpen, doorClose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactionRange))
        {
            if(hit.collider.gameObject.tag == "Door")
            {
                GameObject door = hit.collider.transform.root.gameObject;
                Animator doorAnimation = door.GetComponent<Animator>();
                interactText.SetActive(true);
            }
        }
    }
}
