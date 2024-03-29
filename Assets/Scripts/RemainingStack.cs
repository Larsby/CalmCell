using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemainingStack : MonoBehaviour
{
	/*
 	RemainingStack (7) is the stack with the remaining card deck from start in Klondike
	*/
	//PARAMETERS
	public float ParamBetweenCardsZ = 1.01f;
	public float ParamStartStackCardsZ = -1.21f;
	 

	private double[] rotationValues = {
		0,
		0.3,
		0.7,
		1.1,
		1.5,
		1.9,
		1.8,
		1.4,
		1.1,
		0.8,
		0.5,
		0.2,
		0,
		0.3,
		0.7,
		1.1,
		1.5,
		1.9,
		1.8,
		1.4,
		1.1,
		0.8,
		0.5,
		0.2,
		0,
		0,
		-0.3,
		-0.7,
		-1.1,
		-1.5,
		-1.9,
		-1.8,
		-1.4,
		-1.1,
		-0.8,
		-0.5,
		-0.2,
		0,
		-0.3,
		-0.7,
		-1.1,
		-1.5,
		-1.9,
		-1.8,
		-1.4,
		-1.1,
		-0.8,
		-0.5,
		-0.2,
		0,
		0,
		0,
		-0.4,
		-0.8,
		-1.2,
		-1.6,
		-2.0,
		-2.4,
		-2.8,
		-2.4,
		-2.0,
		-1.6,
		-1.1,
		-0.7,
		-0.3,
		0,
		0,
		0.5,
		0,
		0,
		0,
		0,
		0
	};

	List<GameCard> CardList = new List<GameCard> ();
	public Transform MyParent;

	public RemainingStack ()
	{
	}

	public string GetDrawCardSound (int whichsound)
	{
		switch (whichsound) {
		case 1:
			return "delavand1";
		case 2:
			return "delavand2";
		case 3:
			return "delavand3";
		case 4:
			return "delavand4";
		}
		return "";
	}

	public void AddToStack (GameCard in_card)
	{
		CardList.Add (in_card);
//		in_card.transform.parent = MyParent;
		in_card.transform.SetParent (MyParent, true);
//		in_card.transform.localPosition = Vector3.zero;
//		in_card.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, (float)rotationValues [CardList.Count]));
		in_card.Definition.Stack = 7;

//		Debug.Log ("Cards in Remaining Cardlist now:" + CardList.Count.ToString ());
	}

	 
	public GameCard GetCard (int which)
	{
		return CardList [which];
	}

	public GameCard GetTopCard ()
	{
		if (CountCards () > 0) {
			return CardList [CardList.Count - 1];
		}
		return null;
	}

	public void RemoveTopCard ()
	{
//		Debug.Log ("-------------------------------------------->RemoveTopCard");
		if (CardList.Count > 0) {
			CardList.RemoveAt (CardList.Count - 1);
		}
	}

	public int CountCards ()
	{
		return CardList.Count;
	}

	public void RemoveCardFromStack (int which)
	{
//		Debug.Log ("-------------------------------------------->RemoveCardFromStack");
		if (CountCards () > 0) {
			CardList.RemoveAt (which);
		}
	}

	public bool IsStackEmpty ()
	{
		if (CardList.Count == 0) {
			return true;
		}
		return false;
	}

	public void RemoveAllCards ()
	{
		for (int i = 0; i < CountCards (); i++) {
			CardList.RemoveAt (i);
		}
	}

	public void FixTargetStack ()
	{
		foreach (GameCard c in CardList) {
			c.sourceStack = -1;
			c.targetStack = 7;
		}
	}

	public void OrganizeStack ()
	{
 
		float z = ParamStartStackCardsZ;
		int count = 0;


//		Debug.Log ("Organize Remaining Stack");

		CardList.RemoveRange (0, CardList.Count);	//Clean CardList

//		Debug.Log ("CardList efter rensning =" + CardList.Count.ToString ());
		  
//		Debug.Log ("How many children found? " + MyParent.transform.childCount);
  
		foreach (Transform child in MyParent.transform) {
			CardList.Add (child.gameObject.GetComponentInParent<GameCard> ());
		}
		  
		foreach (GameCard c in CardList) {
			z = -0.03f - count * 0.05f;
			 
			c.Definition.Stack = 7;
			c.Definition.CardOrder = count;
			c.Definition.PrevCard = count - 1;
			c.Definition.NextCard = count + 1;
			c.Definition.FaceUp = false;
			c.transform.SetSiblingIndex (count);

//			c.SetFlyTarget (c.transform.position, new Vector3 (c.transform.position.x, c.transform.position.y, z), 0f, false);		
			c.transform.localPosition = new Vector3 (c.transform.localPosition.x, c.transform.localPosition.y, z);

			count++;
		}

//		Debug.Log ("Remaining CardList contains:" + CardList.Count);
	}

	public void ShuffleCards ()
	{

//		Debug.Log ("Shuffle Remaining cards. Cards=" + CardList.Count.ToString ());

		GameCard tempCard;


		int n = CardList.Count;

		for (int i = 0; i < n; i++) {
			int r = i + (int)(Random.Range(0.0f,1.0f) * (n - i));
			tempCard = CardList [r];
			CardList [r] = CardList [i];
			CardList [i] = tempCard;

		}
		 
		//fix Z-order
		for (int i = 0; i < CardList.Count; i++) {
			CardList [i].transform.localPosition = new Vector3 (
				CardList [i].transform.localPosition.x,
				CardList [i].transform.localPosition.y,
				ParamStartStackCardsZ - (ParamBetweenCardsZ * i)
			);

			//Random subtle rotation if not set
			CardList [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, (Random.Range (-35, 35) / 10))); 

		}

		//Synchronize with stack object
		int count = 0;
		foreach (GameCard c in CardList) {
			foreach (Transform child in MyParent.transform) {
				if (c.Definition.CardColor == child.gameObject.GetComponentInParent<GameCard> ().Definition.CardColor
				    && c.Definition.CardValue == child.gameObject.GetComponentInParent<GameCard> ().Definition.CardValue) { 
					child.SetAsLastSibling ();
					break;
				}
			}
			count++;
		}

		OrganizeStack (); // avoid the dreaded "rotate remain stack slightly after first card flipped" bug
	
	}

	public void Reset() {
		CardList = new List<GameCard> ();
	}
}
