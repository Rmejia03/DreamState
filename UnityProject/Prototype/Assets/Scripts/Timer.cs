using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] float remainingTime;

    // Update is called once per frame
    void Update()
    {
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else if (remainingTime < 0)
            {
                remainingTime = 0;
                gameManager.instance.youLost();
                timerTxt.color = Color.red;
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int second = Mathf.FloorToInt(remainingTime % 60);
            timerTxt.text = string.Format("{0:00}:{1:00}", minutes, second);
        }
    }
}
