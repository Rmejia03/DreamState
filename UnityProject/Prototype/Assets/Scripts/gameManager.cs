using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    //Updated Visuals
    public static gameManager instance;

    [Header("Menu Info")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text goalCount;

    [Header("Player Info")]
    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;
    public GameObject player;
    public playerControl playerScript;

    [Header("UI Info")]
    public GameObject flashShield;
    public GameObject flashDamage;
    public Image HPBar;
    public Image shieldBar;
    public Image fearBar;
    public Image enemyHPBar;

    public bool isPaused;
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player"); 
        //playerScript = player.GetComponent<playerControl>();
        playerSpawnPos = GameObject.FindWithTag("player spawn pos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if(menuActive == null)
            {
                statePause();

                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }

            else if(menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        menuActive.SetActive(isPaused);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        goalCount.text = enemyCount.ToString("F0");

        if (enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    public void youLost()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }
} 
