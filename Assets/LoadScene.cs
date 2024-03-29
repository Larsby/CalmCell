using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int MemorySizeInMB = SystemInfo.systemMemorySize;
		Debug.Log ("sm" + SystemInfo.systemMemorySize + " gm" + SystemInfo.graphicsMemorySize);
		if (MemorySizeInMB > 0 && MemorySizeInMB < 800) {
			
			SceneManager.LoadScene (2);
		} else {
			SceneManager.LoadScene (1);
		}
	}
	

}
