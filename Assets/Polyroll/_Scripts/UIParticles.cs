using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticles : MonoBehaviour {

	[Header(" Particles ")]
	public ParticleSystem[] particles;

	public void PlayParticles(Transform parent = null)
	{
		transform.SetParent(parent);
		transform.SetAsFirstSibling();
		transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

		for (int i = 0; i < particles.Length; i++)
		{
			//particles[i].transform.SetParent(parent);
			
			particles[i].Play();
		}
	}

}
