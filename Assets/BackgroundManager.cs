using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
	public RenderTexture texture1;
	public RenderTexture texture2;
	private RenderTexture current;
	private float time;
	private float secondsToWait = 2f;
	public bool toggle;
	Camera camera;
	// Use this for initialization
	bool firstTime = true;
	public Material mat;
	private bool render = true;

	void Start ()
	{
		current = texture1;
		camera = GetComponent<Camera> ();
		time = Time.time;
		firstTime = true;

	}

	public void TakeSnapShot ()
	{
		render = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (KlondikeGame.instance == null)
			return;
		if (KlondikeGame.instance.GetReady () && firstTime == true) {
			time = Time.time;
			firstTime = false;
		}
		if (time < Time.time) {
			time = Time.time + secondsToWait;
			/*	camera.transform.position = Vector3.MoveTowards (camera.transform.position, new Vector3 (camera.transform.position.x - 0.8f, camera.transform.position.y, camera.transform.position.z), 1f);
				current = camera.targetTexture == texture1 ? texture2 : texture1;
				camera.targetTexture = current;
				*/
			camera.enabled = true;
			render = false;
		}
	}


	void OnRenderImage (RenderTexture src, RenderTexture dest)
	{
		if (render) {
			Graphics.Blit (src, dest, mat);
		}
		camera.enabled = false;

	}
}
