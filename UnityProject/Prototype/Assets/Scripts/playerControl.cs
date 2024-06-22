using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerControl : MonoBehaviour, IDamage
{
    [Header("Player Info")]
    [SerializeField] Animator animate;
    [SerializeField] CharacterController controller;

    [Header("Health/Shield/Fear")]
    [SerializeField] float HP;
    [SerializeField] float shield;
    [SerializeField] int fear;
    [SerializeField] float regenRate;
	public FearVision fearVision;

	[Header("Attack")]
    [SerializeField] float weaponRate;
    [SerializeField] int weaponDamage;
    [SerializeField] int weaponDistance;

    [Header("Movement")]
    [SerializeField] int gravity;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int sprintMod;
    public int speed;

    [Header("Melee")]
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeRange;
    [SerializeField] float meleeCooldown;
    [SerializeField] float meleeAniDuration;
    [SerializeField] float comboResetTime;
    [SerializeField] float comboDamageMultiplier;

    [Header("Items")]
    [SerializeField] GameObject itemModels;
    public inventoryManager InventoryManager;

    [Header("Doors")]
    [SerializeField] int rayLength = 5;
    [SerializeField] LayerMask layerInteract;
    [SerializeField] string excludeLayerName = "Player";
    [SerializeField] Image crosshair = null;

    private DoorSoundAnimation raycastObject;
    [SerializeField] KeyCode openDoorKey = KeyCode.E;
    private const string interactableTag = "InteractiveObject";
    private bool crosshairActive;
    private bool doOnce;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    int jumpCount;
    float HPOrig;
    int fearOrig;
    int selectedItem;
    int selectedGun;
    float shieldOrig;
    //bool isShooting;
    bool isMeleeing;
    bool isRegen;

    //bool inHazard = false;
    bool isFlashing = false;
    //float hazardFlashDelay = 1f;

    float nextMeleeTime;
    int attackSeq = 0;
    int successfulHits = 0;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        shieldOrig = shield;
        fearOrig = fear;

        animator = GetComponent<Animator>();

        updateHPBarUI();
        updateShieldUI();
        updateFearUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            float animateSpeed = controller.velocity.normalized.magnitude;
        }

       if (fearVision.intensity > 0)
            {
                updateFearUI();
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
            HandleBackhandMelee();
            //selectItem();
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
            if (fearVision.FearRisingCo == null && fearVision.rising)
			 {
			     fearVision.FearRisingCo = StartCoroutine(fearVision.FearRising());
			 }
			 if (fearVision.ResetFearCo == null && !fearVision.rising)
			 {
			     fearVision.ResetFearCo = StartCoroutine(fearVision.ResetFear());
			 }
        }
        else
        {
			if (fearVision.FearRisingCo != null)
			{
				StopCoroutine(fearVision.FearRisingCo);
				fearVision.FearRisingCo = null;
			}
			if (fearVision.ResetFearCo != null)
			{
				StopCoroutine(fearVision.ResetFearCo);
				fearVision.ResetFearCo = null;
			}
			
		}
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
            animate.SetBool("IsJumping", false);
        }

        Sprint();

        //moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
        //                (Input.GetAxis("Vertical") * transform.forward);
        //controller.Move(moveDirection * speed * Time.deltaTime);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = (horizontalInput * transform.right) + ( verticalInput * transform.forward);
        controller.Move(moveDirection * speed * Time.deltaTime);


        float isMoving = moveDirection.magnitude;

        animate.SetBool("Move Forward", verticalInput > 0);
        animate.SetBool("Move Backward", verticalInput < 0);
        animate.SetBool("Move Right", horizontalInput > 0);
        animate.SetBool("Move Left", horizontalInput < 0);

        if(isMoving == 0)
        {
            animate.SetBool("Move Forward", false );
            animate.SetBool("Move Backward", false);
            animate.SetBool("Move Right", false);
            animate.SetBool("Move Left", false);
        }

        //Dodge Right
        if(Input.GetButtonDown("Dodge") && horizontalInput > 0)
        {
            animate.SetTrigger("Dodge Right");
            StartCoroutine(Dodge(transform.right));
        }

        //Dodge Left
        if(Input.GetButtonDown("Dodge") && horizontalInput < 0)
        {
            animate.SetTrigger("Dodge Left");
            StartCoroutine(Dodge(-transform.right));
        }

        //Dodge Backwards
        if(Input.GetButtonDown("Dodge") && verticalInput < 0)
        {
            animate.SetTrigger("Dodge Backwards");
            StartCoroutine(Dodge(-transform.forward));
        }
        //if(animate != null)
        //{
        //    animate.SetFloat("IsMoving", isMoving);
        //}

        //if (Input.GetButton("Fire1") && !isShooting)
        //{
        //    StartCoroutine(Shoot());
        //}

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
            animate.SetBool("IsJumping", true);
        }

        playerVelocity.y -= gravity * Time.deltaTime;   
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void slowDownPlayer(int slowSpeed)
    {
        speed -= slowSpeed;
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
        //isShooting = true;
        RaycastHit hit;
        //int mask = (1 << LayerMask.NameToLayer(excludeLayerName)) | layerInteract.value;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponDistance))
        {
            Debug.Log(hit);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(weaponDamage);
            }
            itemStats selectedItem = InventoryManager.GetSelectedItem();
            if (selectedItem != null && selectedItem.hitEffect != null)
            {
                Instantiate(selectedItem.hitEffect, hit.point, Quaternion.identity);
            }

            itemStats itemStats = InventoryManager.GetSelectedItem();
            if (selectedItem != null && selectedItem.hitEffect != null)
            {
                Instantiate(selectedItem.hitEffect, hit.point, Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(weaponRate);
        //isShooting = false;

    }

    public void DoorInteraction()
    {
        RaycastHit hit;
        int mask = (1 << LayerMask.NameToLayer(excludeLayerName)) | layerInteract.value;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayLength, mask))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            if (hit.collider.CompareTag(interactableTag))
            {
                if (!doOnce)
                {
                    raycastObject = hit.collider.gameObject.GetComponent<DoorSoundAnimation>();
                    CrosshairChange(true);
                }

                crosshairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(openDoorKey))
                {
                    raycastObject.PlayAnimation();
                }
            }
            else
            {
                if (crosshairActive)
                {
                    CrosshairChange(false);
                    doOnce = false;
                }
            }
        }
        else
        {
            if (crosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }
        }
    }

    void CrosshairChange(bool on)
    {
        if(on && !doOnce)
        {
            crosshair.color = Color.white;
        }
        else
        {
            crosshair.color = Color.red;
            crosshairActive = false;
        }
    }

    void HandleBackhandMelee()
    {
        if (Input.GetButtonDown("Fire1") && !isMeleeing && Time.time >= nextMeleeTime)
        {
 
            nextMeleeTime = Time.time + meleeCooldown;

            StartCoroutine(MeleeAttack());
        }
    }

    IEnumerator Dodge(Vector3 dodgeDirection)
    {
        float dodgeDistance = 3f;

        float dodgeDuration = 0.5f;

        float startTime = Time.time;

        while(Time.time < startTime + dodgeDuration)
        {
            controller.Move(dodgeDirection * (dodgeDistance * Time.deltaTime / dodgeDuration));

            yield return null;
        }
        
    }
    IEnumerator MeleeAttack()
    {
        if (!isMeleeing)
        {
            isMeleeing = true;

            attackSeq++;
            if (attackSeq > 3)
                attackSeq = 1;

            switch (attackSeq)
            {
                case 1:
                    animate.SetTrigger("Backhand Melee");
                    break;
                case 2:
                    animate.SetTrigger("Slash Melee");
                    break;
                case 3:
                    animate.SetTrigger("Stab Melee");
                    break;
            }

            yield return new WaitForSeconds(meleeAniDuration / 2);

            bool hit = DetectMeleeHit();

            yield return new WaitForSeconds(meleeAniDuration / 2);

            isMeleeing = false;

            if (hit)
            {
                successfulHits++;
                if (successfulHits >= 3)
                {
                    int originalDamage = meleeDamage;
                    meleeDamage = (int)(meleeDamage * comboDamageMultiplier);

                    DealMeleeDamage();

                    meleeDamage = originalDamage;

                    successfulHits = 0;
                }
                else
                {
                    DealMeleeDamage();
                }
            }
            else
            {
                successfulHits = 0;
            }
            
        }
    }



    void DealMeleeDamage()
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

    bool DetectMeleeHit()
    {
        bool hit = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                hit = true;
                break;
            }
        }
        return hit;
    }

    public void takeDamage(float amount, bool slowFlash = false)
    {
        if(shield <= 0)
        {
            HP -= amount;
            updateHPBarUI();
            if (!fearVision.rising)
            {
                fearVision.FearRisingCo = fearVision.StartCoroutine(fearVision.FearRising());
            }

            if(slowFlash)
            {
                StartCoroutine(flashScreenSlow());
            }
            else
            {
                StartCoroutine(flashScreen());
            }
        }
        else
        {
            shield -= amount;
            //updateHPBarUI();
            updateShieldUI();
            if(slowFlash)
            {
                StartCoroutine(flashShieldSlow());
            }
            else
            {
                StartCoroutine (flashShield());
            }
        }
        if(HP <= 0)
        {
            gameManager.instance.youLost();
        }
  //      if (shield <= 0)
  //      {
  //          HP -= amount;
  //          updateHPBarUI();
  //          StartCoroutine(flashScreen());
  //      }
  //      else
  //      {
  //          shield -= amount;
  //          updateShieldUI();
  //          StartCoroutine(flashShield());
		//}
        
  //      if (HP <= 0)
  //      {
  //          gameManager.instance.youLost();
  //      }
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
    IEnumerator flashScreenSlow()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            gameManager.instance.flashDamage.SetActive(true);
            yield return new WaitForSeconds(1f);
            gameManager.instance.flashDamage.SetActive(false);
            yield return new WaitForSeconds(1f);
            isFlashing = false;
        }
    }

    IEnumerator flashShieldSlow()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            gameManager.instance.flashShield.SetActive(true);
            yield return new WaitForSeconds(1f);
            gameManager.instance.flashShield.SetActive(false);
            yield return new WaitForSeconds(1f);
            isFlashing = false;
        }
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
    void updateFearUI()
    {
        gameManager.instance.fearBar.fillAmount = fearVision.intensity;
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
        if(InventoryManager == null)
        {
            return;
        }

        //inventoryManager.AddItem(item);

        itemStats selectedItem = InventoryManager.GetSelectedItem();

        if(selectedItem != null)
        {
            weaponDamage = item.weaponDmg;
            weaponDistance = item.weaponDistance;
            weaponRate = item.weaponSpeed;

            itemModels.GetComponent<MeshFilter>().sharedMesh = selectedItem.itemModel.GetComponent<MeshFilter>().sharedMesh;
            itemModels.GetComponent<MeshRenderer>().sharedMaterial = selectedItem.itemModel.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }

    //void selectItem()
    //{
    //    if(Input.GetAxis("Mouse ScrollWheel") > 0)
    //    {
    //        Debug.Log("scroll up");
    //        inventoryManager.SelectNextItem();
    //        changeItem();
    //    }
    //    if(Input.GetAxis("Mouse ScrollWheel") < 0)
    //    {
    //        Debug.Log("scroll down");
    //        inventoryManager.SelectPreviousItem();
    //        changeItem();
    //    }
    //}

    void useItem()
    {
        if (Input.GetButtonDown("Use Health"))
        {
            
            if (InventoryManager.healingItemIndex == 0)
            {
                //Debug.Log("No healing items left");
                return;
            }
            else if(HP == HPOrig)
            {
                return;
            }
            else
            {
                //Debug.Log($"Healing with amount: {InventoryManager.healingItem.healthAmt}");
                healPlayer(InventoryManager.healingItem.healthAmt);
                updateHPBarUI();
                InventoryManager.healingItemIndex -= 1;
                InventoryManager.UpdateCount();
                //Debug.Log($"Healing items left: {InventoryManager.healingItemIndex}");
            }
        }
        else if (Input.GetButtonDown("Use Fear"))
        {
            if(InventoryManager.fearItemIndex <= 0)
            {
                return;
            }
            else if (fear == fearOrig)
            {
                return;
            }
            else
            {
                fearMeter(InventoryManager.fearItem.fearAmt);
                updateFearUI();
                InventoryManager.fearItemIndex -= 1;
                InventoryManager.UpdateCount();
            }   
        }
    }

        //public void changeItem()
        //{
        //    itemStats selectedItem = inventoryManager.GetSelectedItem();

        //    if (selectedItem != null)
        //    {
        //        weaponDamage = selectedItem.weaponDmg;
        //        weaponDistance = selectedItem.weaponDistance;
        //        weaponRate = selectedItem.weaponSpeed;

        //        itemModels.GetComponent<MeshFilter>().sharedMesh = selectedItem.itemModel.GetComponent<MeshFilter>().sharedMesh;
        //        itemModels.GetComponent<MeshRenderer>().sharedMaterial = selectedItem.itemModel.GetComponent<MeshRenderer>().sharedMaterial;
        //    }
        //    else if (selectedItem == null)
        //    {
        //        itemModels.GetComponent<MeshFilter>().sharedMesh = null;
        //        itemModels.GetComponent<MeshRenderer>().sharedMaterial = null;
        //    }

        //}



    public void healPlayer(int amount)
    {
        HP = Mathf.Min(HPOrig, HP + amount);
        updateHPBarUI();
        Debug.Log($"Player healed by {amount}. Current HP: {HP}");
    }

    public void fearMeter(int amount)
    {
        fear = Mathf.Min(fearOrig, fear - amount);
        updateFearUI();
    }
}

