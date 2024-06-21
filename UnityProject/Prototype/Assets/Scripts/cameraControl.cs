using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVerticalMin, lockVerticalMax;
    [SerializeField] bool invertY = false;
    [SerializeField] Transform playerTransform;
    [SerializeField] float cameraDistance = 4f;
    [SerializeField] float verticalCamera = 1.5f;
    [SerializeField] LayerMask collision;
  
    float rotationX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sens;

        if (invertY)
            rotationX += mouseY;
        else
            rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, lockVerticalMin, lockVerticalMax);
        Vector3 cameraPosition = playerTransform.position - transform.forward * cameraDistance + Vector3.up * verticalCamera;

        RaycastHit hit;
        if(Physics.Linecast(playerTransform.position, cameraPosition, out hit, collision))
        {
            cameraPosition = hit.point;
        }

        transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * 10);
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);

    }
}
