using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnMobile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		gameObject.SetActive(false);
		#endif
		#if UNITY_IPHONE
		gameObject.SetActive(false);
		#endif
		#if  UNITY_EDITOR
	//	gameObject.SetActive(false);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
