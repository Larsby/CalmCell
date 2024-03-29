using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowMenuOnTouch : MonoBehaviour
{

	private bool toggle;
	public GameObject thePnl;
	public Button button;
	public Sprite show;
	public Sprite hide;
	public KlondikeGame gameManager;

	void Start ()
	{
		toggle = true;
		button = GetComponent<Button> ();
	}

	public void ToggleMenu ()
	{
		gameManager.ToggleSettingsPanel ();

		if (toggle) {
			button.image.sprite = hide;
		} else { 
			button.image.sprite = show;
		}
		toggle = !toggle;


	}

	void OnMouseDown ()
	{
		ToggleMenu ();
	}


}
