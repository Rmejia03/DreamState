using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] float remainingTime;
    public bool timerOn;

    private void Start()
    {
        if (PlayerPrefs.HasKey("remainingTime"))
        {
            remainingTime = PlayerPrefs.GetFloat("remainingTime");
        }
        if(PlayerPrefs.HasKey("timerOn"))
        {
            timerOn = PlayerPrefs.GetInt("timerOn") == 1;
        }

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            timerOn = false;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            timerOn = true;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            timerOn = true;
        }
        UpdateTimer();
    }
    // Update is called once per frame
    void Update()
    {
        if (timerOn && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if(remainingTime <= 0)
            {
                remainingTime = 0;
                gameManager.instance.youLost();
                timerTxt.color = Color.red;
            }
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int second = Mathf.FloorToInt(remainingTime % 60);
        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, second);
    }

    public void startTimer()
    {
        timerOn = true;
        SaveTimer();
        //PlayerPrefs.SetInt("timerOn", 1);
    }

    public void stopTimer()
    {
        timerOn = false;
        SaveTimer();
        //PlayerPrefs.SetInt("timerOn", 0);
    }

    public void timerStartScene()
    {
        SaveTimer();
        SceneManager.LoadScene(2);
        //timerOn = true;
    }

    public void timerStopScene()
    {
        SaveTimer();
        SceneManager.LoadScene(1);
        //timerOn = false;
    }

    void SaveTimer()
    {
        PlayerPrefs.SetFloat("remainingTime", remainingTime);
        PlayerPrefs.SetInt("timerOn", timerOn ? 1 : 0);
    }
}
