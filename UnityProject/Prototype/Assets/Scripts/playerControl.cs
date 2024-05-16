using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerControl : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] int gravity;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int sprintMod;
    [SerializeField] int speed;
    [SerializeField] int HP;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    int jumpCount;
    int HPOrig;
    bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI(); 
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        Movement();
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        Sprint();

        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
                        (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDirection * speed * Time.deltaTime);

        if(Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
        }

        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }

        playerVelocity.y -= gravity * Time.deltaTime;   
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod; 
        }
    }



    IEnumerator Shoot()
    {
        isShooting = true;
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance))
        {
            Debug.Log(hit);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }

        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
       
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashScreen());


        if (HP <= 0)
        {
            gameManager.instance.youLost();
        }
    }

    IEnumerator flashScreen()
    {
        gameManager.instance.flashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.flashDamage.SetActive(false);
    }

    void updatePlayerUI()
    {
        gameManager.instance.HPBar.fillAmount = (float)HP / HPOrig;
    }

}
