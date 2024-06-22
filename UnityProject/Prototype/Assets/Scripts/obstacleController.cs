using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleController : MonoBehaviour, IDamage
{
    public bool isSpiderWeb;
    public bool isThorns;
    public bool isMushroom;

    public float HP;

    playerControl Player;
    //private int sprintOrig;
    bool isPoking;


	private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {            
            Player = other.GetComponent<playerControl>();
            
            Player.Stuck = true;
            Player.stopSprint();
            if (Player != null && isSpiderWeb)
            {
                Player.slowDownPlayer(4);
            }
			if (Player != null && isThorns)
			{
				Player.slowDownPlayer(4);
                StartCoroutine(thornPoke());
			}
		}
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player.sprintMod = Player.sprintOrig;
			Player.Stuck = false;
			Player.speed = Player.speedOrig;
            if(Player != null && isSpiderWeb)
            {
                Player = null;
            }
			if (Player != null && isThorns)
			{				
				Player = null;
			}
		}
    }

    public void takeDamage(float amount, bool slowFlash = false)
    {
        HP -= amount;

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }


    IEnumerator thornPoke()
    {
            gameManager.instance.player.GetComponent<IDamage>().takeDamage(1);
            yield return new WaitForSeconds(2);
    }
}
