using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [SerializeField] float timerDuration = 300f;
    public float timeRemaining;
    public bool isTimerRunning = false;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 0)
            {
                isTimerRunning = false;
                timeRemaining = 0;
                LoseScenario();
            }
        }
    }

    public void StartTimer()
    {
        timeRemaining = timerDuration;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void LoseScenario()
    {
        gameManager.instance.youLost();
    }
}
