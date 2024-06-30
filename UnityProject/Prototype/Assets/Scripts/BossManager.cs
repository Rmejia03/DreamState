using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public int totalBosses = 2;
    private int defeatedBosses = 0;

    public GameObject winScreen;
    //public GameObject finalPortal;

    public GameObject boss1;
    public GameObject boss2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            defeatedBosses = PlayerPrefs.GetInt("defeatedBosses", 0);
            Debug.Log("Initial defeatedBosses: " + defeatedBosses);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateBossUI();
    }

    public void BossDefeated(int num)
    {
        defeatedBosses += num;
        Debug.Log("Boss defeated. Current defeatedBosses: " + defeatedBosses);

        PlayerPrefs.SetInt("defeatedBosses", defeatedBosses);
        PlayerPrefs.Save();

        UpdateBossUI();
        CheckAllBossesDefeated();
    }

    public void UpdateBossUI()
    {
        if (defeatedBosses >= 1)
        {
            boss1.SetActive(true);
        }
        else if (defeatedBosses >= 2)
        {
            boss1.SetActive(true);
            boss2.SetActive(true);
        }
    }

    private void CheckAllBossesDefeated()
    {
        if (defeatedBosses >= totalBosses)
        {
           // finalPortal.SetActive(true);
            //ShowWinScreen();
        }
    }

    public void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            gameManager.instance.statePause();
        }
    }
}
