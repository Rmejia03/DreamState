using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        GameObject selectedSkinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/selectedSkin.prefab");

        if(selectedSkinPrefab != null )
        {
            GameObject clonedPlayer = Instantiate(selectedSkinPrefab, spawnPoint.position, spawnPoint.rotation);
            clonedPlayer.tag = "Player";
            clonedPlayer.layer = LayerMask.NameToLayer("Player");

            gameManager.instance.player = clonedPlayer;
            //gameManager.instance.playerScript = clonedPlayer.GetComponent<playerControl>();
            playerControl playerScript = clonedPlayer.GetComponent<playerControl>();

            if (playerScript != null)
            {

                playerScript.meleeAttackSound = Resources.Load<AudioClip>("Audio/MeleeAttack");
                playerScript.heavyAttackSound = Resources.Load<AudioClip>("Audio/HeavyAttack");
                playerScript.defendHitSound = Resources.Load<AudioClip>("Audio/DefendHit");

                AudioSource audioSource = clonedPlayer.GetComponent<AudioSource>();

                if (audioSource == null)
                {
                    audioSource = clonedPlayer.AddComponent<AudioSource>();
                }
                playerScript.audioSource = audioSource;

                gameManager.instance.playerScript = playerScript;
                inventoryManager.Instance.InitializeInventoryForPlayer(clonedPlayer);
            }
        }
    }
}
