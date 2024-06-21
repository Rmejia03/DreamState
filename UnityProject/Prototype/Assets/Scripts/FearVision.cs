using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FearVision : MonoBehaviour
{
	public float intensity = 0;
	PostProcessVolume volume;
	Vignette vignette;

	public Coroutine FearRisingCo;
	public Coroutine ResetFearCo;
	public bool rising;

	// Start is called before the first frame update
	void Start()
	{
		volume = GetComponent<PostProcessVolume>();
		volume.profile.TryGetSettings<Vignette>(out vignette);
		
		
		if (!vignette)
		{
			print("Error, No Vignette");
		}
		else
		{
			vignette.enabled.Override(false);
		}
	}

	// Update is called once per frame
	//void Update()
	//{
	//	if (Input.GetKeyUp(KeyCode.I))
	//	{
	//		StartCoroutine(FearRising());
	//	}
	//	if (Input.GetKeyUp(KeyCode.O))
	//	{
	//		StartCoroutine(ResetFear());
	//	}
	//}

	public IEnumerator FearRising()
	{
		if (ResetFearCo != null)
		{
			StopCoroutine(ResetFearCo);
			ResetFearCo = null;
		}

		if (vignette.enabled == false)
		{
			vignette.enabled.Override(true);
		}
		
		rising = true;
		while (vignette.intensity.value < 1)
		{
			
			intensity += .0005f;
			if(intensity>1)
				intensity = 1;
			vignette.intensity.Override(intensity);
			yield return null;
		}
	}

	public IEnumerator ResetFear()
	{
		if (FearRisingCo != null)
		{
			StopCoroutine(FearRisingCo);
			FearRisingCo = null;
		}
		rising = false;
		while (vignette.intensity.value > 0)
		{
			intensity -= .001f;
			if(intensity<0)
				intensity = 0;
			vignette.intensity.Override(intensity);
			yield return null;
		}
		vignette.enabled.Override(false) ;
	}
}
