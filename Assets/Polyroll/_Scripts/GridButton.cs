using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class GridButton : MonoBehaviour {

	[Header(" Stuff ")]
	public Text indexText;
	public Image image;

	public void Roll()
	{
		GetComponent<Animator>().Play("Roll");
	}

	public void Appear()
	{
		image.gameObject.SetActive(true);
		GetComponent<Animator>().Play("Appear");
	}

}
