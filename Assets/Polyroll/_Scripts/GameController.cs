using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[Header(" Debug ")]
	public bool deletePrefs;

	[Header(" Canvas Groups ")]
	public CanvasGroup MENU;
	public CanvasGroup GAME;
	public CanvasGroup SETTINGS;
	public Image fadeImage;
	public float fadeSpeed;
	CanvasGroup[] canvases;

	[Header(" Game Buttons ")]
	public GameObject backButton;
	public GameObject nextButton;

	[Header(" Grid Container ")]
	public Transform gridContainer;
	public static int activeImage;

	[Header(" Sprites ")]
	public Sprite[] sprites;

	[Header(" Managers ")]
	public RotationCenter rotationCenter;
	public AdMobManager adMobManager;
	public UnityAdsManager unityAdsManager;

	[Header(" Effects ")]
	public RectTransform uiParticlesParent;

	[Header(" Sounds ")]
	public AudioSource openImageSound;
	public AudioSource imageFinishedGame;
	public AudioSource imageFinishedMenu;

	[Header(" Ads ")]
	public GameObject noAdsButton;

	// Use this for initialization
	void Start () {
		
		int noAds = PlayerPrefs.GetInt("NOADS");

		if(noAds == 1)
		{
			noAdsButton.SetActive(false);
		}

		// Enable the fade panel
		fadeImage.gameObject.SetActive(true);

		// Store the canvases into an array
		canvases = new CanvasGroup[] { MENU, GAME, SETTINGS};
		SetGrid();

		// Add the listeners to the grid buttons
		AddListeners();

	}
	
	// Update is called once per frame
	void Update () {
		
		if(deletePrefs)
			PlayerPrefs.DeleteAll();

	}

	public void SetGrid()
	{
		// Hide all the canvas groups except the grid
		//MyUtils.HideAllCGs(canvases);
		//MyUtils.EnableCG(MENU);
		StartCoroutine(ShowCanvasGroupCoroutine(MENU));

		
	}

	public void SetGrid(bool finishedImage)
	{
		// Hide all the canvas groups except the grid
		//MyUtils.HideAllCGs(canvases);
		//MyUtils.EnableCG(MENU);
		StartCoroutine(ShowCanvasGroupCoroutine(MENU, finishedImage));
	
	}

	public void SetSettings()
	{
		// Show the settings panel
		StartCoroutine(ShowCanvasGroupCoroutine(SETTINGS));
	}

	IEnumerator ShowCanvasGroupCoroutine(CanvasGroup destinationCG, bool finished = false)
	{

		fadeImage.raycastTarget = true;

		while(fadeImage.color.a < 1)
		{
			Color c = fadeImage.color;
			c.a += Time.deltaTime * fadeSpeed;
			fadeImage.color = c;
			yield return new WaitForSeconds(Time.deltaTime);
		}

		// At this point, the screen is completely white
		// Manage the canvases
		MyUtils.HideAllCGs(canvases);
		MyUtils.EnableCG(destinationCG);

		yield return new WaitForSeconds(0.25f);

		while(fadeImage.color.a > 0)
		{
			Color c = fadeImage.color;
			c.a -= Time.deltaTime * fadeSpeed;
			fadeImage.color = c;
			yield return new WaitForSeconds(Time.deltaTime);
		}

		fadeImage.raycastTarget = false;
		
		// If we have finised the image, play the particles
		if(finished)
		{
			uiParticlesParent.GetComponent<UIParticles>().PlayParticles(gridContainer.GetChild(activeImage));
			gridContainer.GetChild(activeImage).GetComponent<GridButton>().Appear();
			imageFinishedMenu.Play();
		}

		yield return null;
	}

	void AddListeners()
	{
		int k = 0;
		foreach (Button b in gridContainer.GetComponentsInChildren<Button>())
		{
			int _k = k;

			if(_k < sprites.Length)
			{
				b.onClick.AddListener(delegate { SetGame(_k); });
				b.GetComponent<GridButton>().indexText.text = (_k+1).ToString();
				b.GetComponent<GridButton>().image.sprite = sprites[_k];

				// Check if this image has been unlocked or not to show it or not
				int imageState = PlayerPrefs.GetInt("Image" + _k);
				if(imageState == 1)
				{
					b.GetComponent<GridButton>().indexText.text = "";
				}
				else
				{
					b.GetComponent<GridButton>().image.gameObject.SetActive(false);
				}
			}
			else
			{
				b.gameObject.SetActive(false);
			}
			
			k++;
		}
	}

	public void ImageFinished()
	{
		// Hide the back button
		backButton.SetActive(false);

		// Show the next button
		nextButton.SetActive(true);

		PlayerPrefs.SetInt("Image" + activeImage, 1);

		// Setup button as finished
		gridContainer.GetChild(activeImage).GetComponent<GridButton>().image.color = Color.white;
		gridContainer.GetChild(activeImage).GetComponent<GridButton>().indexText.text = "";

		// Play a finished sound
		imageFinishedGame.Play();


	}

	public void SetGame(int spriteIndex)
	{
		// Hide the grid panel and show the game one
		// Or just fade in the game panel maybe
		// Set the active index
		activeImage = spriteIndex;

		// Animate the button
		gridContainer.GetChild(spriteIndex).GetComponent<GridButton>().Roll();

		// Hide the back button
		backButton.SetActive(true);

		// Show the next button
		nextButton.SetActive(false);

		StartCoroutine(ShowCanvasGroupCoroutine(GAME));
		
		// Set the sprite to play with
		rotationCenter.imageSR.sprite = sprites[spriteIndex];

		// Then split the image
		rotationCenter.SplitImage();

		// Play an opening sound
		openImageSound.Play();
	}

	public void PurchaseNoAds()
	{
		// Save the preference
		PlayerPrefs.SetInt("NOADS", 1);

		// Disable Unity ads & Admob
		adMobManager.enabled = false;
		unityAdsManager.enabled = false;
		noAdsButton.SetActive(false);
	}
}
