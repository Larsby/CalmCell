using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
	// this class is used by the Apple tv to handle navigation.
	public int activeItem = 0;
	private string[] choiches = new string[]{ "MenuButton", "Restart", "MusicOnButton", "MusicOffButton" };

	public void ChangeCurrentMenuItem (int in_choice)
	{
		
		activeItem = in_choice;

		Button[] buttons;

		buttons = GameObject.Find (choiches [in_choice]).GetComponents<Button> ();
		foreach (Button butt in buttons) {
			butt.Select ();
		}
	}

}
