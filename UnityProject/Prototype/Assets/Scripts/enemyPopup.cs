using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyPopup : MonoBehaviour
{
   
    public float popUpSpeed;
    public float popUpHeight;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    
    bool isPoppingUp = false;
    bool isAboveGround = false;


    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        targetPosition = new Vector3(initialPosition.x, initialPosition.y + popUpHeight, initialPosition.z);

        transform.position = initialPosition;

        agent = GetComponent<NavMeshAgent>();
        if(agent != null )
        {
            agent.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPoppingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, popUpSpeed * Time.deltaTime);

            if(transform.position == targetPosition )
            {
                isPoppingUp = false;
                isAboveGround = true;
                if(agent!=null)
                {
                    agent.enabled = true;
                }
            }
        }
    }

    public void StartPoppingUp()
    {
        if(!isAboveGround)
        {
            isPoppingUp = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isAboveGround)
        {
            StartPoppingUp();
        }
    }
}
