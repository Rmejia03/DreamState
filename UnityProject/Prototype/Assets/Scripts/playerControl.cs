using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class playerControl : MonoBehaviour, IDamage
{
    [Header("Player Info")]
    //[SerializeField] Animator animate;
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

    [Header("items")]
    //[SerializeField] List<itemStats> inventory = new List<itemStats>();
    [SerializeField] GameObject itemModels;
    public inventoryManager inventoryManager;

    public KeyCode useItemKey = KeyCode.E;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    int jumpCount;
    int HPOrig;
    int selectedItem;
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
            //Debug.Log("i can move);

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * weaponDistance, Color.red);

            if (shield < shieldOrig && HPOrig == HP && !isRegen)
            {
                StartCoroutine(RegenShield());
            }

            Movement();
            selectItem();
            useItem();

           /*if(!isMeleeing && Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(Shoot());

            }
            else
            {
                Debug.Log(!isMeleeing + " " + Input.GetButtonDown("Fire1"));
            }*/

            //UpdateAnimation();
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

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
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

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponDistance))
        {
            Debug.Log(hit);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(weaponDamage);
            }

        }
        yield return new WaitForSeconds(weaponRate);
        isShooting = false;

    }

    //IEnumerator MeleeAttack()
    //{
    //    if (!isMeleeing)
    //    {
    //        isMeleeing = true;

    //        animate.SetTrigger("Hit");

    //        yield return new WaitForSeconds(meleeAniDuration);

    //        DetectMeleeHit();

    //        //nextMeleeTime = Time.time + meleeCooldown;

    //        isMeleeing = false;
    //    }
    //}

    //void DetectMeleeHit()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRange);

    //    foreach (Collider collider in colliders)
    //    {
    //        if (collider.CompareTag("Enemy"))
    //        {
    //            IDamage dmg = collider.GetComponent<IDamage>();

    //            if (dmg != null)
    //            {
    //                dmg.takeDamage(meleeDamage);
    //            }
    //        }

    //    }
    //}





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

    public void getItemStats(itemStats item)
    {
        if(inventoryManager == null)
        {
            return;
        }

        inventoryManager.AddItem(item);

        itemStats selectedItem = inventoryManager.GetSelectedItem();

        if(selectedItem != null)
        {
            weaponDamage = item.weaponDmg;
            weaponDistance = item.weaponDistance;
            weaponRate = item.weaponSpeed;

            itemModels.GetComponent<MeshFilter>().sharedMesh = selectedItem.itemModel.GetComponent<MeshFilter>().sharedMesh;
            itemModels.GetComponent<MeshRenderer>().sharedMaterial = selectedItem.itemModel.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }

    void selectItem()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log("scroll up");
            inventoryManager.SelectNextItem();
            changeItem();
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Debug.Log("scroll down");
            inventoryManager.SelectPreviousItem();
            changeItem();
        }
    }

    void useItem()
    {
        if(Input.GetKeyDown(useItemKey))
        {
            itemStats selectedItem = inventoryManager.GetSelectedItem();

            if(inventoryManager.IsHealingItemSelected())
            {
                healPlayer(selectedItem.healthAmt);
                inventoryManager.RemoveItem(selectedItem);
            }
        }
    }

    void changeItem()
    {
        itemStats selectedItem = inventoryManager.GetSelectedItem();

        if(selectedItem != null)
        {
            weaponDamage = selectedItem.weaponDmg;
            weaponDistance = selectedItem.weaponDistance;
            weaponRate = selectedItem.weaponSpeed;

            itemModels.GetComponent<MeshFilter>().sharedMesh = selectedItem.itemModel.GetComponent<MeshFilter>().sharedMesh;
            itemModels.GetComponent<MeshRenderer>().sharedMaterial = selectedItem.itemModel.GetComponent<MeshRenderer>().sharedMaterial;
        }    
    }

    void healPlayer(int amount)
    {
        HP = Mathf.Min(HPOrig, HP + amount);
        updateHPBarUI();
    }
}

