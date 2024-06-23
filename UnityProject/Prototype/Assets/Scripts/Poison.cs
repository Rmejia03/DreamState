using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField] private int poisonDamage = 2;
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ApplyPoison(other.GetComponent<IDamage>()));
        }
    }

    private IEnumerator ApplyPoison(IDamage player)
    {
        float time = 0f;

        while (time < duration)
        {
            player.takeDamage(poisonDamage);
            yield return new WaitForSeconds(1f);
            time += 1;
        }
    }
}
