using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraOutUntilInvisible : MonoBehaviour
{

	private bool done = false;
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{


		if (done == false) {
			
			Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 0.1f);
		
		}
	}

	void OnBecameInvisible ()
	{
		done = true;
		KlondikeGame.instance.SetReady (true); // zoomed out
	}
}
