using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] float wallStickForce = 5.0f;
    [SerializeField] float climbRaycast = 1.0f;
    [SerializeField] LayerMask wallLayer;

    Rigidbody body;
    NavMeshAgent agent;
    bool isClimbing;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        body.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isClimbing)
        {
            CheckForWall();
        }
        else
        {
            StickToSurface();
        }
        
    }

    void CheckForWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, climbRaycast, wallLayer))
        {
            Debug.Log("Wall detected");
            StartClimbing(hit.normal);
        }
    }

    void StartClimbing(Vector3 climb)
    {
        isClimbing = true;
        body.useGravity = false;
        agent.enabled = false;

        Quaternion targetRotation = Quaternion.LookRotation(-climb, transform.up);
        transform.rotation = targetRotation;

       

    }
    void StickToSurface()
    {
       
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 1.0f, wallLayer))
            {
                body.AddForce(-hit.normal * wallStickForce, ForceMode.Acceleration);
                Debug.DrawRay(transform.position, -transform.up * 1.0f, Color.red);
            }
        
      
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == wallLayer && !isClimbing)
        {
            //isClimbing = true;
            //body.useGravity = false;
            //agent.enabled = false;
            StartClimbing(collision.contacts[0].normal);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == wallLayer && isClimbing)
        {
            isClimbing = false;
            body.useGravity = true;
            agent.enabled = true;
        }
    }
}
