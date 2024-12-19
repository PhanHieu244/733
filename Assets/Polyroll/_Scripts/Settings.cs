using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	[Header(" Sound ")]
	public Image soundImage;
	public Sprite soundOnSprite;
	public Sprite soundOffSprite;
	public AudioSource[] sounds;
	public AudioSource buttonSound;
	bool soundState = true;

	// Use this for initialization
	void Start () {
		
		int soundOn = PlayerPrefs.GetInt("SOUND");
		if(soundOn == 1)
		{
			ChangeSoundState();
		}

	}
	

	public void ChangeSoundState()
	{
		soundState = !soundState;

		if(soundState)
		{
			// Enable the sounds
			soundImage.sprite = soundOnSprite;
			SetSoundsState(1);

			// Play a button sound
			buttonSound.Play();
		}
		else
		{
			// Disable the sounds
			soundImage.sprite = soundOffSprite;
			SetSoundsState(0);
		}
	}

	public void SetSoundsState(int volume)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			sounds[i].volume = volume;
		}
	}


}
