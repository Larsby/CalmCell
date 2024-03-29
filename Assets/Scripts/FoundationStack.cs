using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoundationStack : MonoBehaviour
{
	/*
 	FoundationStack is one of four cardstacks where the cards should be added in correct color and order
	*/
	//PARAMETERES
	public float ParamBetweenCardsZ = 0.01f;
	public float ParamStartStackCardsZ = -0.11f;

	private float rotcompx=0;		//Compensate for the original rotation of the texture

	public int Stack;			//9-12, one for each color
	private int CurrentColor;	//Once set only this color is valid for the rest of the game
	private int CurrentValue=-1;
	private int SelectedCard=-1;

	List<GameCard> CardList = new List<GameCard> ();
 
	public Transform MyParent;
	public bool isIntermediate;

	public FoundationStack()
	{ 
	}

	public void ResetStuff() {
		CurrentValue=-1;
		SelectedCard=-1;
	}


	public void ResetCurrentValue() {
		CurrentValue=-1;
	}
		

	public void SetStack(int in_stack) {
		Stack = in_stack;
	}

	public bool PreventMoveTop() {
		if (isIntermediate)
			return false;
		return CountCards () == 1;
	}

	public void AddToStack(GameCard incard, bool forceAdd = false)  // forceAdd for debug purposes only
	{
//		Debug.Log ("AddToStack");
//		Debug.Log ("Cards in foundationstack (before add): " + CardList.Count);
//		Debug.Log ("CurrentValue="+CurrentValue);

		if((incard.Definition.CardValue==1 && CardList.Count==0) || (forceAdd) || (isIntermediate)) {
			CurrentColor = incard.Definition.CardColor;
			CurrentValue = incard.Definition.CardValue;
			incard.Definition.Stack = Stack;
			incard.transform.parent = MyParent;
			CardList.Add(incard);
//			Debug.Log ("Position Ace"); 
		}
		else if(incard.Definition.CardColor==CurrentColor) {
//			Debug.Log ("Correct color");
			if(incard.Definition.CardValue == CurrentValue+1) {
//				Debug.Log ("Position other than ace");
//				incard.transform.rotation = Quaternion.Euler (new Vector3 (0+rotcompx, 0,(Random.Range(-13,13)/10))); 
				CurrentValue = incard.Definition.CardValue;
				incard.Definition.Stack = Stack;
				incard.transform.parent = MyParent;
				CardList.Add(incard); 
			}
		}

		OrganizeStack ();
	}
	 
	public bool IsCardValid(GameCard incard) 
	{
		if (isIntermediate) {
			return CardList.Count == 0;
		}

		if(incard.Definition.CardValue==1 && CurrentValue==-1) {
//			Debug.Log ("First Ace");
			if (incard.Definition.CardValue > CurrentValue) {
				return true;
			}
		}
		if(incard.Definition.CardColor==CurrentColor) {
//			Debug.Log ("Not Ace");
//			if(incard.Definition.CardValue > CurrentValue) {
			if(incard.Definition.CardValue == CurrentValue + 1 || incard.Definition.CardValue == 1 && CurrentValue == -1) {
				return true;
			}
		}
//		Debug.Log ("Not valid");
		return false;
	}

	public void RemoveTopCardFromFoundation() 
	{
		CardList.RemoveAt (CardList.Count - 1);

		foreach (GameCard c in CardList) {
			CurrentValue = c.Definition.CardValue;
		}

	}

	public bool IsFoundationFull() {
		if (CardList.Count == 13) {
			return true;
		}
		return false;
	}

	public bool IsStackEmpty() {
		if (CardList.Count == 0) {
			return true;
		}
		return false;
	}

	public int CountCards() {
		return CardList.Count;
	}

	public GameCard GetCard(int which) {
		if (CardList.Count>0 && which<CardList.Count) {
			return CardList [which];
		}
		Debug.Log ("ERROR IN GetCard. THIS SHOULD NOT BE REACHED");
		return null;
	}

	public GameCard GetTopCard() {
		if (CountCards() > 0) {
			return CardList [CardList.Count - 1];
		}
		return null;
	}


	public void RemoveAllCards() {
		for (int i = 0; i < CountCards (); i++) {
			CardList.RemoveAt(i);
		}
		CurrentValue = -1;
		
	}

	public void SelectCard(bool changeSize) {
		 
		if (!IsStackEmpty ()) {
			if (changeSize) {
				UnSelectCard ();
				CardList [CardList.Count-1].SelectCard ();
			}
			SelectedCard = CardList.Count-1;
		}
	}

	public void UnSelectCard() {
		if (!IsStackEmpty () && SelectedCard!=-1) {
			CardList [SelectedCard].UnSelectCard ();
			SelectedCard = -1;
		}
	}

	public GameCard GetSelectedCard() {
		if (!IsStackEmpty () && SelectedCard != -1) {
			return CardList [SelectedCard];
		} else
			return null;
	}

	public void RemoveSelectedCard() {
		if (SelectedCard != -1) {
			CardList.RemoveAt (SelectedCard);
		}
		else {
			Debug.Log ("ERROR: Didn't remove card ");
		}
		SelectedCard = -1;

		foreach (GameCard c in CardList) {
			CurrentValue = c.Definition.CardValue;
		}
	}

	public void OrganizeStack() {

		float x;
		float y;
		float z = ParamStartStackCardsZ;
		int count = 0;
		GameCard lastcard = null;

		//Sort list so everything is in order (Ace->King)
		if (CardList.Count > 1) {
			CardList.Sort (delegate(GameCard value1,GameCard value2) {
				if (value1.Definition.CardValue < value2.Definition.CardValue) {
					return -1;
				} else if (value1.Definition.CardValue == value2.Definition.CardValue) {
					return 0;
				} else {
					return 1;
				}
			}
			);
		}

		if (!IsStackEmpty ()) {
			CurrentColor = CardList[CardList.Count-1].Definition.CardColor;
			CurrentValue = CardList[CardList.Count-1].Definition.CardValue;

//			Debug.Log ("FOUNDATION: OrganizeStack:-------------------->Cards=" + CardList.Count.ToString ());
//			Debug.Log ("FOUNDATION: OrganizeStack:-------------------->CurrentValue=" + CurrentValue.ToString ());

			foreach (GameCard c in CardList) {
				c.Definition.CardOrder = count;
				c.Definition.Stack = Stack;
				c.Definition.NextCard = -1;
				c.Definition.PrevCard = -1;
		
//				c.transform.rotation = Quaternion.Euler(0+rotcompx,0,0);
//				c.transform.localScale = new Vector3 (2.5f, 3.5f, 0.01f);
				c.transform.position = new Vector3 (c.transform.position.x,c.transform.position.y,0f);	//This resets position before changing to local pos below. Don't remove!

				x = 0f;
				y = 0f;

				if (lastcard != null) {
					z = lastcard.transform.localPosition.z - 0.11f;
				} else {
					z = -0.11f;
				}
				c.transform.localPosition = new Vector3 (x,y,z);
						
				lastcard = c;

				count++;
			}

		}
	}

	public void Reset() {
		CardList = new List<GameCard> ();
		rotcompx=0;	
		CurrentValue=-1;
		SelectedCard=-1;
	}

}