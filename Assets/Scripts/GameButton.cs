using UnityEngine;
using System.Collections;

public class GameButton : MonoBehaviour
{
	public string Message;

	void OnMouseDown()
	{
		//Debug.Log ("Mouse down on button");
		transform.parent.gameObject.GetComponent<KlondikeGame>().OnButton(Message);
	}
}
