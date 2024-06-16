using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
	[SerializeField] GameObject arrow;
	[SerializeField] Transform shootPOS;
	[SerializeField] float resetRate;
	bool isActive;

	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine(trapTriggered());
		}
	}

	IEnumerator trapTriggered()
	{
		
		Instantiate(arrow, shootPOS.position, shootPOS.rotation);
	
		yield return resetRate;
		
	}
}
