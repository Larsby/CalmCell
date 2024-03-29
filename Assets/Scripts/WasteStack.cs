using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class WasteStack : MonoBehaviour
{
	/*
 	WasteStack (8) is the stack with the discarded card in Klondike
	*/
	 
	//PARAMETERES
	public float ParamBetweenCardsZ = 0.01f;
	public float ParamStartStackCardsZ = -0.11f;

 
	private int CurrentCard = -1;

	List<GameCard> CardList = new List<GameCard> ();
 
	public Transform MyParent;

	public WasteStack ()
	{
	}

	public void AddToStack (GameCard in_card, bool isTouch)
	{
//		Debug.Log ("ADD TO WASTE STACK. isTouch=" + isTouch);
		if (in_card != null)
		{
			in_card.transform.parent = MyParent;

			if (!isTouch)
			{
				if (CountCards () > 0)
					in_card.transform.localPosition = new Vector3 (in_card.transform.localPosition.x, in_card.transform.localPosition.y, GetTopCard ().transform.localPosition.z - 0.05f);
				else
					in_card.transform.localPosition = new Vector3 (in_card.transform.localPosition.x, in_card.transform.localPosition.y, -0.03f);

			}

			//		in_card.transform.localRotation=Quaternion.identity;
			in_card.Definition.Stack = 8;
			in_card.Definition.CardOrder = CardList.Count;
			CardList.Add (in_card);

	
			CardList [CardList.Count - 1].Definition.FaceUp = true;
		}
	}

	public GameCard GetCard (int which)
	{
		try
		{
			return CardList [which]; 
		} catch (Exception e)
		{

			return null;
		}  
	}

	public GameCard GetTopCard ()
	{
		if (CountCards () > 0)
		{
			return CardList [CardList.Count - 1];
		}
		return null;
	}

	public void RemoveTopCard ()
	{
		if (CountCards () > 0)
		{
			CardList.RemoveAt (CardList.Count - 1);
		}
	}

	public int CountCards ()
	{
		return CardList.Count;
	}

	public void RemoveCardFromStack (int which)
	{
		CardList.RemoveAt (which);
	}

	public bool IsStackEmpty ()
	{
		if (CardList.Count == 0)
		{
			return true;
		}
		return false;
	}

	public void RemoveAllCards ()
	{
		for (int i = 0; i < CountCards (); i++)
		{
			CardList.RemoveAt (i);
		}
	}

	public void SelectCard ()
	{
//		Debug.Log ("I Waste - SelectCard. CurrentCard =" + CurrentCard.ToString ());

		if (!IsStackEmpty ())
		{
			UnSelectCard ();
			CardList [CardList.Count - 1].SelectCard ();
		}
	}

	public void UnSelectCard ()
	{
		if (!IsStackEmpty ())
		{
			CardList [CardList.Count - 1].UnSelectCard ();
		}
	}


	public void OrganizeStack ()
	{

		WasteStack thisStack;
		GameCard tempcard = null;
		Transform lastcard = null;
		float x;
		float y;
		float z;
		int count = 0;

//		Debug.Log ("Organize Wastestack - cards here=" + CardList.Count.ToString ());
		  
		CardList.RemoveRange (0, CardList.Count);	//Clean CardList
		 
		thisStack = GameObject.Find ("WasteStack").GetComponent<WasteStack> ();
		foreach (Transform child in thisStack.transform)
		{
			tempcard = child.gameObject.GetComponentInParent<GameCard> ();

			tempcard.Definition.Stack = 8;
			tempcard.Definition.NextCard = -1;
			tempcard.Definition.PrevCard = -1;
			tempcard.Definition.FaceUp = true;
			tempcard.Definition.Clickable = true;
			tempcard.Definition.CardOrder = count;

			//	child.position = new Vector3 (child.position.x,child.position.y,0f);	//This resets position before changing to local pos below. Don't remove!
			//	child.rotation = Quaternion.Euler(0,0,0);
			x = child.localPosition.x;
			y = child.localPosition.y;
			if (lastcard != null)
			{
				z = lastcard.localPosition.z - 0.05f;
			} else
			{
				z = -0.03f;
			}
			child.localPosition = new Vector3 (x, y, z);

			lastcard = child;

			CardList.Add (tempcard);

			count++;
		}

	}

	public void Reset() {
		CardList = new List<GameCard> ();
		CurrentCard = -1;
	}
}