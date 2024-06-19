using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleController : MonoBehaviour
{
    public bool isSpiderWeb;
    public bool isThorns;
    public bool isMushroom;

    public float HP;

    playerControl Player;
    private int speedOrig;

    bool isPoking;

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
			if (Player != null && isThorns)
			{
				speedOrig = Player.speed;
				Player.slowDownPlayer(2);
                StartCoroutine(thornPoke());
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
			if (Player != null && isThorns)
			{
				Player.speed = speedOrig;
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
