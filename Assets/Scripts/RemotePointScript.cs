using UnityEngine;
using System.Collections;

public class RemotePointScript : MonoBehaviour {
	 
	Vector2 mypos = new Vector2 (0, 0);

	float scr_width=0;
	float scr_height=0;
	float speedx=30.0f;
	float speedy=20.0f;

	// Use this for initialization
	void Start () {
		scr_width = Screen.width;
		scr_height = Screen.height;

		//Debug.Log ("scr_width=" + scr_width);
		//Debug.Log ("scr_height=" + scr_height);
	}
	 
	void Update () {
		 

		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) { //Start drag
			//				Debug.Log ("REMOTE: Click down");
			}

			if (Input.GetTouch (0).phase == TouchPhase.Ended || Input.GetTouch (0).phase == TouchPhase.Canceled) { //Stop drag
				//				Debug.Log ("REMOTE: Click up");
			}

			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				mypos = mypos + Input.GetTouch (0).deltaPosition;

				//Debug.Log ("REMOTE: mypos.x=" + mypos.x);
				//Debug.Log ("REMOTE: mypos.y=" + mypos.y);

				float tempx = speedx * mypos.x / scr_width;
				float tempy = speedy * mypos.y / scr_height;

				if (tempx > 8.8f)	// 9.6
					tempx = 8.8f;
				if (tempx < -7.8f)	// -8.5
					tempx = -7.8f;
				if (tempy > 4.6f)	// 4.8
					tempy = 4.6f;
				if (tempy < -6.5f)	// -7.3
					tempy = -6.5f;



				this.transform.position = new Vector3 (tempx, tempy, -1.0f);

				//				Debug.Log ("REMOTE: position.x=" + this.transform.position.x);
				//				Debug.Log ("REMOTE: position.y=" + this.transform.position.y);
				//				Debug.Log ("REMOTE: position.z=" + this.transform.position.z);
			}

		}
	}
}
