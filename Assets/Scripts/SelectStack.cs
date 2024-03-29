using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SelectStack : MonoBehaviour
{
	/*
 	SelectStack is the stack with the currently selected (in hand) cards in Klondike
	*/
	 
	//PARAMETERES
	public float ParamBetweenHiddenCards = 0.3f;
	public float ParamBetweenCardsZ = 0.3f;
	public float ParamStartStackCardsZ = -0.11f;

	public float Originalx = 0f;
	public float Originaly = 0f;
	public float Originalz = 0f;

	private int lastValidStack = 0;

	List<GameCard> CardList = new List<GameCard> ();
 
	public Transform MyParent;

	public int DebugAntalKort;

	public SelectStack ()
	{
	}

 
	void Update ()
	{
		DebugAntalKort = CardList.Count;
	}

	public void AddToStack (GameCard in_card, bool isTouch)
	{
//		Debug.Log ("Selectstack.AddtoStack. MyParent=" + MyParent.name.ToString ());
//		Debug.Log ("incard==" + in_card.transform.name.ToString ());

		if (isTouch) {
			in_card.SetNoFly ();
		}
		in_card.transform.parent = MyParent;
		lastValidStack = in_card.Definition.Stack;
		in_card.Definition.Stack = -1;
		CardList.Add (in_card);
		 
	}

	public void resetToCardFly ()
	{
		foreach (GameCard card in CardList) {	
			card.SetFly ();
//			card.SetResetAnimation ();
		}
	}

	// misol
	public void resetToBackAnimation ()
	{
		foreach (GameCard card in CardList) {	
//			card.SetFly ();
			card.SetResetAnimation ();
		}
	}


	public GameCard GetCard (int which)
	{
		try {
			return CardList [which]; 
		} catch (Exception e) {
			return null;
		}  
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
		if (CountCards () > 0) {
			CardList.RemoveAt (CardList.Count - 1);
		}

	}

	public int CountCards ()
	{
		return CardList.Count;
	}

	public void RemoveCardFromStack (int which)
	{
		try {		
			CardList.RemoveAt (which);
	 
		} catch (Exception e) {
			//trying to remove out of range here
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

	public void UnselectAllCards ()
	{
		foreach (GameCard card in CardList) {	
			card.UnSelectCard ();
		}
	}

	//Compensate for perspective when clicking on leftmost or rightmost cards
	private float CompensatePerspectiveX (int clickedstack)
	{
		if (clickedstack > -1 && clickedstack < 7) {
			if (clickedstack == 0)
				return 0.25f;
			else if (clickedstack == 1)
				return 0.1f;
			else if (clickedstack == 5)
				return -0.1f;
			else if (clickedstack == 6)
				return -0.25f;
			else
				return 0; 
		} else {
			return 0;
		}
	}

	public void FlipCardStack (Vector3 axis)
	{
		foreach (GameCard card in CardList) {	
			card.FlipCard (axis);
		}
	}


	public void OrganizeCards (bool isTouch)
	{
		int count = 0;
		float x;
		float y;
		float z = ParamStartStackCardsZ;

//		Debug.Log ("OrganizeCards i SelectStack. Antal kort:" + CardList.Count);

		if (CardList.Count > 0) {	//Move selectstack
			if (isTouch)
				this.transform.position = new Vector3 (Originalx, Originaly, -2.0f);//CardList [CardList.Count - 1].transform.position;
			else
				this.transform.position = new Vector3 (Originalx + CompensatePerspectiveX (lastValidStack), Originaly, -2.0f);//CardList [CardList.Count - 1].transform.position;
		}

		foreach (GameCard card in CardList) {	

			card.transform.position = new Vector3 (card.transform.position.x, card.transform.position.y, 0f);	//This resets position before changing to local pos below. Don't remove!

			x = 0;
			y = -(count) * ParamBetweenHiddenCards;
			z = -5 - ParamStartStackCardsZ - (count * ParamBetweenCardsZ);

			card.transform.localPosition = new Vector3 (x, y, 0.0f);
			card.transform.position = new Vector3 (card.transform.position.x, card.transform.position.y, z);	 
//			card.transform.localScale = new Vector3(1.5f, 1.5f, 10);
			count++;
		}
	}

	public void PlayLiftAnim ()
	{
		foreach (GameCard card in CardList) {	
			card.PlayLiftAnim();
		}
	}

}
