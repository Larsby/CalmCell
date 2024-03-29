using UnityEngine;
using System.Collections;

public class TakeScreenShot : MonoBehaviour
{

	// Use this for initialization
	public bool capture = true;
	public string screenshotname = "Zenklondike";
	private static int index = 1;

	void Start ()
	{
		index = 1;
	}



	void Capture ()
	{
		ScreenCapture.CaptureScreenshot (Application.dataPath + "/InGameShots/" + screenshotname + index + ".png");
		index++;

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("space")) {
			capture = true;
		}
		if (capture) {
			Capture ();
			capture = false;
		}
	}
}