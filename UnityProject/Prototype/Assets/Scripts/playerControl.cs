using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerControl : MonoBehaviour, IDamage
{
    [Header("Player Info")]
    [SerializeField] Animator animate;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] CharacterController controller;
 
    [Header("Health/Shield")]
    [SerializeField] int HP;
    [SerializeField] float shield;
    [SerializeField] float regenRate;

    [Header("Attack")]
    [SerializeField] float weaponRate;
    [SerializeField] int weaponDamage;
    [SerializeField] int weaponDistance;

    [Header("Movement")]
    [SerializeField] int gravity;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int sprintMod;
    [SerializeField] int speed;

    [Header("Melee")]
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeRange;
    [SerializeField] float meleeCooldown;
    [SerializeField] int meleeAniDuration;

    [Header("weapons")]
    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] GameObject weaponModel;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    int jumpCount;
    int HPOrig;
    int selectedWeapon;
    float shieldOrig;
    bool isShooting;
    bool isMeleeing;
    bool isRegen;


    float nextMeleeTime;


    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        shieldOrig = shield;

        updateHPBarUI(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null)
        {
            float animateSpeed = agent.velocity.normalized.magnitude;
        }
       
        //Prevents hit damage on pause
        if (!gameManager.instance.isPaused)
        {
            //Debug.Log("i can move");
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * weaponDistance, Color.red);
            if (shield < shieldOrig && HPOrig == HP && !isRegen)
            {
                StartCoroutine(RegenShield());
            }

            Movement();

           if(!isMeleeing && Input.GetButtonDown("Fire1"))
            {  
                StartCoroutine(MeleeAttack()); 
            }
            else
            {
                Debug.Log(!isMeleeing + " " + Input.GetButtonDown("Fire1"));
            }

            UpdateAnimation();
        }
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

        //if(Input.GetButton("Fire1") && !isShooting)
        //{
        //    StartCoroutine(Shoot());
        //}

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

    //IEnumerator Shoot()
    //{
    //    isShooting = true;
    //    RaycastHit hit;

    //    if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance))
    //    {
    //        Debug.Log(hit);

    //        IDamage dmg = hit.collider.GetComponent<IDamage>();

    //        if(hit.transform != transform && dmg != null)
    //        {
    //            dmg.takeDamage(shootDamage);
    //        }

    //    }
    //    yield return new WaitForSeconds(shootRate);
    //    isShooting = false;
       
    //}
    
    IEnumerator MeleeAttack()
    {
        if (!isMeleeing)
        {
            isMeleeing = true;

            animate.SetTrigger("Hit");

            yield return new WaitForSeconds(meleeAniDuration);

            DetectMeleeHit();

            //nextMeleeTime = Time.time + meleeCooldown;

            isMeleeing = false;
        }
    }

    void DetectMeleeHit()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                IDamage dmg = collider.GetComponent<IDamage>();

                if (dmg != null)
                {
                    dmg.takeDamage(meleeDamage);
                }
            }

        }
    }

 
  


    public void takeDamage(int amount)
    {
        if (shield <= 0)
        {
            HP -= amount;
            updateHPBarUI();
            StartCoroutine(flashScreen());
        }
        else
        {
            shield -= amount;
            updateShieldUI();
            StartCoroutine(flashShield());
		}
        
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
    
	IEnumerator flashShield()
	{
		gameManager.instance.flashShield.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		gameManager.instance.flashShield.SetActive(false);
	}
    
    IEnumerator RegenShield()
    {
        isRegen = true;
        if (shield < shieldOrig)
        {
            yield return new WaitForSeconds(5);
            shield += regenRate;
            updateShieldUI();
            shield = Mathf.Clamp(shield, 0, shieldOrig);
        }
        isRegen=false;
	}

	void updateHPBarUI()
	{
		gameManager.instance.HPBar.fillAmount = (float)HP / HPOrig;
	}

	void updateShieldUI()
	{
		gameManager.instance.shieldBar.fillAmount = shield / shieldOrig;
	}

    public void spawnPlayer()
    {
        HP = HPOrig; 
        shield = shieldOrig;

        updateHPBarUI();
        updateShieldUI();

        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void getWeaponStats(weaponStats weapon)
    {
        weaponList.Add(weapon);

        selectedWeapon = weaponList.Count - 1;

        weaponDamage = weapon.weaponDmg;
        weaponDistance = weapon.weaponDistance;
        weaponRate = weapon.weaponSpeed;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weapon.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weapon.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectWeapon()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weaponList.Count - 1)
        {
            selectedWeapon++;
            changeWeapon();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > weaponList.Count)
        {
            selectedWeapon--;
            changeWeapon();
        }
    }

    void changeWeapon()
    {
        weaponDamage = weaponList[selectedWeapon].weaponDmg;
        weaponDistance = weaponList[selectedWeapon].weaponDistance;
        weaponRate = weaponList[selectedWeapon].weaponSpeed;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[selectedWeapon].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[selectedWeapon].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
    void UpdateAnimation()
    {
        if(moveDirection.magnitude > 0)
        {
            animate.SetTrigger("Move");
        }
    }
}

