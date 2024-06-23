using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public int totalBosses = 2;
    private int defeatedBosses = 0;

    public GameObject winScreen;
    public GameObject finalPortal;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void BossDefeated()
    {
        defeatedBosses++;
        CheckAllBossesDefeated();
    }

    private void CheckAllBossesDefeated()
    {
        if(defeatedBosses >= totalBosses)
        {
            finalPortal.SetActive(true);
        }
    }

    public void ShowWinScreen()
    {
        if(winScreen != null)
        {
            winScreen.SetActive(true);
        }
    }
}
