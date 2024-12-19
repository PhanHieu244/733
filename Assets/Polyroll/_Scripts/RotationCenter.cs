using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCenter : MonoBehaviour {

	[Header(" Managers ")]
	public GameController gameController;

	[Header(" Control ")]
    public float rotationSpeed;
    public float smooth;
	public float finishSmooth;
    public float inertia;
    Vector3 pressedPos, actualPos;
    float targetRotX, targetRotY;
	bool canRotate;
	bool started;

	[Header(" Image Management ")]
	public GameObject spriteRendererPrefab;
	public SpriteRenderer imageSR;
	public Animator imageAnimator;
	int gridSize;
	public int imageSize;
	Camera mainCamera;

	// Use this for initialization
	void Start () {
		
		mainCamera = Camera.main;
		//SplitImage(imageSR.sprite);

	}
	
	// Update is called once per frame
	void Update () {

		if(started)
		{


			if(canRotate)
				ManageControl();
			else
			{
				//transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, Vector3.zero, finishSmooth);
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, finishSmooth);


				if(Quaternion.Angle(Quaternion.identity, transform.rotation) < 1)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, finishSmooth*3);

					// Destroy all the parts and show the original image
					while(transform.childCount > 1)
					{
						Transform t = transform.GetChild(1);
						t.parent = null;
						Destroy(t.gameObject);
					}

					imageSR.gameObject.SetActive(true);
					imageAnimator.Play("Bump");
					started = false;

					transform.rotation = Quaternion.identity;

					gameController.ImageFinished();

					

					// Show the share buttons and so on

				}
			}
		}
	}

	public void DestroyParts()
	{
		// Destroy all the parts and show the original image
		while(transform.childCount > 1)
		{
			Transform t = transform.GetChild(1);
			t.parent = null;
			Destroy(t.gameObject);
		}

		started = false;
		transform.rotation = Quaternion.identity;
	}

	#region Control Management

	void ManageControl()
	{


		// Check if we are close enough to the original rotation
		if(Quaternion.Angle(Quaternion.identity, transform.rotation) < 5f)
		{
			//canRotate = false;
		}




		if (Input.GetMouseButtonDown(0))
		{
			pressedPos = GetCorrectedMousePos();
			actualPos = pressedPos;

			targetRotX = 0;
			targetRotY = 0;
		}

		else if (Input.GetMouseButton(0))
		{

		if(Mathf.Abs(transform.rotation.x - Quaternion.identity.x) < 0.01f &&
			Mathf.Abs(transform.rotation.y - Quaternion.identity.y) < 0.01f)
			{
				canRotate = false;
			}

			// Store the actual pos
			actualPos = GetCorrectedMousePos();

			// Store the difference in x between the two
			float dx = actualPos.x - pressedPos.x;
			float dy = actualPos.y - pressedPos.y;

			float targetX = dx / (float)Screen.width * -rotationSpeed;
			float targetY = dy / (float)Screen.height * rotationSpeed;

			targetRotX = Mathf.Lerp(targetRotX, targetX, smooth) * Time.deltaTime;
			targetRotY = Mathf.Lerp(targetRotY, targetY, smooth) * Time.deltaTime;

			//transform.Rotate(new Vector3(targetRotX * Time.deltaTime, targetRotY * Time.deltaTime, 0));
			transform.RotateAround(Vector3.zero, Vector3.right, targetRotY);
			transform.RotateAround(Vector3.zero, Vector3.up, targetRotX);

			// Reset the difference
			pressedPos = GetCorrectedMousePos();
		}
		else
		{
			targetRotX = Mathf.Lerp(targetRotX, 0, inertia);
			targetRotY = Mathf.Lerp(targetRotY, 0, inertia);

			transform.RotateAround(Vector3.zero, Vector3.right, targetRotY);
			transform.RotateAround(Vector3.zero, Vector3.up, targetRotX);
		}
	}

	Vector3 GetCorrectedMousePos()
    {
        float screenWidth = Screen.width;
        float screeHeight = Screen.height;

        Vector3 correctedPos = new Vector3(Input.mousePosition.x - (screenWidth / 2), Input.mousePosition.y - (screeHeight / 2), 0);

        return correctedPos;
    }

	#endregion





	#region Image Management
	
	public void SplitImage()
	{
		// Set a random grid size
		List<int> sizes = new List<int>() {10, 12, 14, 16, 20};

		gridSize = sizes[Random.Range(0, sizes.Count)];
		
		// Setup the sprite mask
		imageSR.GetComponent<SpriteMask>().sprite = imageSR.sprite;

		// Split the image
		StartCoroutine(SplitImageCoroutine(imageSR.sprite));
	}

	IEnumerator SplitImageCoroutine(Sprite s)
	{
		yield return new WaitForEndOfFrame();

		// First off, let's try and split the image in 2 halfs
		// Let's say that the images will have a 300x300 size
		
		
		Vector2 spriteScreenPos = mainCamera.WorldToScreenPoint(imageSR.transform.position);
		Vector2 screenBounds = (Vector2)mainCamera.WorldToScreenPoint(imageSR.bounds.size) - spriteScreenPos;
		float ratio = (float)screenBounds.x / (float)s.texture.width;

		// Calculate the pixels per unit
		int ppu = Mathf.FloorToInt(100 * ratio);
		
		Rect readRect = new Rect(spriteScreenPos.x - screenBounds.x/2, spriteScreenPos.y - screenBounds.y/2, screenBounds.x,screenBounds.y);

		float xStep = readRect.width / (float)gridSize;
		float yStep = readRect.height / (float)gridSize;

		int step = imageSize / gridSize;

		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{
				Rect rect = new Rect(readRect.x + (x*xStep), readRect.y + (y * yStep), xStep, yStep);
				//Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
				//tex.ReadPixels(rect, 0, 0, false);
				//tex.Apply();

				
				
				// TODO : This is working
				//Rect spriteRect = new Rect(0, 0, tex.width, tex.height);
				//Sprite sprite = Sprite.Create(tex, spriteRect, new Vector2(0.5f, 0.5f), ppu);

				Rect spriteRect = new Rect(x*step, y*step, step, step);
				Sprite sprite = Sprite.Create(imageSR.sprite.texture, spriteRect, new Vector2(0.5f, 0.5f), 97);

				// Instantiate the new image
				//Vector3 spawnPos = new Vector3((x - gridSize/2) / ppu, (y - gridSize/2) / ppu,(Random.Range(-1f,1f)));
				Vector3 spawnPos = Camera.main.ScreenToWorldPoint(new Vector3(rect.x + xStep/2, rect.y + yStep/2));
				spawnPos.z = Random.Range(-1f,1f);
				GameObject imageInstance = Instantiate(spriteRendererPrefab, spawnPos, Quaternion.identity,transform);
				imageInstance.GetComponent<SpriteRenderer>().sprite = sprite;
			}
		}

		imageSR.gameObject.SetActive(false);
		
		transform.localEulerAngles = new Vector3(Random.Range(-360f,360f), Random.Range(-360f,360f), Random.Range(-360f,360f));
		//transform.localEulerAngles = new Vector3(50,50,50);

		yield return new WaitForSeconds(Time.deltaTime);

		canRotate = true;

		yield return new WaitForSeconds(Time.deltaTime);

		started = true;

		yield return null;
	}

	#endregion

}
