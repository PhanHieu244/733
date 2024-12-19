using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class MyUtils {

    public static Texture2D LoadTexture(string path)
    {
        if (!File.Exists(path)) { Debug.Log("File does not exist "); return null;}
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D result = new Texture2D(1, 1);
        FileInfo finfo = new FileInfo(path);
        result.name = finfo.Name;
        result.LoadImage(bytes);
        return result;
    }

    // Returns a random gameobject from an array
	public static GameObject GetRandomGameobjectFromArray(GameObject[] objectsArray)
	{
		// Initialize the random gameobject
		GameObject randomGameobject;

		// Get a random index
		int randomIndex = Random.Range(0, objectsArray.Length);

		// Store the random gameobject
		randomGameobject = objectsArray[randomIndex];

		// Return it
		return randomGameobject;
	}

	// Returns a random gameobject from a list
	public static GameObject GetRandomGameobjectFromList(List<GameObject> objectsArray)
	{
				// Initialize the random gameobject
		GameObject randomGameobject;

		// Get a random index
		int randomIndex = Random.Range(0, objectsArray.Count);

		// Store the random gameobject
		randomGameobject = objectsArray[randomIndex];

		// Return it
		return randomGameobject;
	}

	// Format an int to string with a space each thousand
	public static string FormatAmountString(int amount)
	{
		string formattedAmount = "";

		if(amount < 1000)
		{
			formattedAmount = amount.ToString();
		}
		else if(amount >= 1000 && amount < 1000000)
		{
			formattedAmount = (amount/1000).ToString() + " " + amount.ToString().Substring(amount.ToString().Length - 3, 3);
		}

		return formattedAmount;
	}

	// Returns the screen ratio
	public static float GetScreenRatio()
	{
		float w = Screen.currentResolution.width;
		float h = Screen.currentResolution.height;

		float ratio = w/h;

		return ratio;
	}		

	public static void EnableCG(CanvasGroup cg)
	{
		cg.alpha = 1;
		cg.interactable = true;
		cg.blocksRaycasts = true;
	}

	public static void DisableCG(CanvasGroup cg)
	{
		cg.alpha = 0;
		cg.interactable = false;
		cg.blocksRaycasts = false;
	}

	public static void HideAllCGs(CanvasGroup[] cgs)
	{
		for (int i = 0; i < cgs.Length; i++)
		{
			DisableCG(cgs[i]);
		}
	}

	public static void RateIos(string gameId)
	{
		Application.OpenURL("itms-apps://itunes.apple.com/app/id" + gameId);
	}
}


