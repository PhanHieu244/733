using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	[Header(" Settings ")]
	Transform mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.LookAt(mainCamera);
		transform.forward = mainCamera.forward;
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
	}
}
