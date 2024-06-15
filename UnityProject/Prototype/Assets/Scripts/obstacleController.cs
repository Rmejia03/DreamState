using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleController : MonoBehaviour
{
    public bool isSpiderWeb;
    public bool isThorns;
    public bool isMushroom;

    playerControl Player;
    private int speedOrig;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player = other.GetComponent<playerControl>();
            if (Player != null && isSpiderWeb)
            {
                speedOrig = Player.speed;
                Player.slowDownPlayer(2);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Player != null && isSpiderWeb)
            {
                Player.speed = speedOrig;
                Player = null;
            }
        }
    }
}
