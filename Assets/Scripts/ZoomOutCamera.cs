using UnityEngine;
using System.Collections;

public class ZoomOutCamera : MonoBehaviour
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
			Vector3 viewPos = Camera.main.WorldToViewportPoint (transform.position);
			if ((viewPos.x < 0.0f || viewPos.x > 0.995f) || (viewPos.y < 0.05f || viewPos.y > 0.995f)) {
				KlondikeGame.instance.SetReady (true); // zoomed out.
				done = true;

			} else {
//				Camera.main.fieldOfView += 0.2f;
			}
		}
	}
}
