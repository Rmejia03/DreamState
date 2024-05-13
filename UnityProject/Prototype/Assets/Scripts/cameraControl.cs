using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVerticalMin, lockVerticalMax;
    [SerializeField] bool invertY;
  

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

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);

    }
}
