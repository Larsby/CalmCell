using UnityEngine;
using System.Collections;
//using Facebook.Unity;

using UnityEngine.SocialPlatforms;
using System.IO;
using System;

public class Utilities : MonoBehaviour
{
	public string game;
	public string HighScore;
	public UniRate r;
	public  void Share ()
	{
		
		/*
		Facebook.Unity.FB.FeedShare ("",
			new Uri ("http://www.pastille.se/"),
			"Get "+game,
			null, null);
	*/
	}

	public  void More() {
		Application.OpenURL ("http://www.pastille.se");
	}
	
	public  void Rate ()
	{
		Application.OpenURL("itms://itunes.apple.com/app/id1449153397");

//		r.ShowPrompt ();
	}
	
	
}