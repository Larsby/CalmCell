using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColumnStack : MonoBehaviour
{
	/*
 	ColumnStack is one of seven cardstacks where the cards start from in Klondike
 	*/

	//PUBLIC
	public bool AutoCardTurn = false;

	public Transform MyParent;
	public int rotationType = 1;
	public int Stack;
	//0-6

	//CONSTANTS
	const float FlipFlyTime = 0.65f;
	 
	//PRIVATE
	private int SelectedCard = -1;
	private int LastColor = -1;
	private int LastValue = -1;
	private int CurrentCard = -1;

	private float ParamYBetweenHiddenCards = 0.15f;
	private bool firstTimeEver = true;

	private ColumnStack thisColumn;
	private KlondikeGame thisGame;
	List<GameCard> CardList = new List<GameCard> ();

	//VALUEARRAYS
	private double[] rotationValues1 = {
		0,
		0.3,
		0.7,
		1.1,
		1.5,
		1.9,
		2.3,
		2.7,
		2.7,
		2.6,
		2.2,
		1.8,
		1.4,
		1.1,
		0.8,
		0.5,
		0.2,
		0,
		-0.4,
		-0.7,
		-1.0,
		-0.7,
		0,
		0,
		0
	};
	private double[] rotationValues2 = {
		0,
		-0.3,
		-0.7,
		-1.1,
		-1.5,
		-1.9,
		-2.3,
		-2.7,
		-2.7,
		-2.6,
		-2.2,
		-1.8,
		-1.4,
		-1.1,
		-0.8,
		-0.5,
		-0.2,
		0,
		0.4,
		0.7,
		1.0,
		0.7,
		0,
		0,
		0
	};
	private double[] rotationValues3 = {
		0,
		0,
		0,
		0.1,
		0.1,
		0.2,
		0.3,
		0.7,
		1.1,
		1.5,
		1.9,
		2.3,
		2.7,
		2.7,
		2.6,
		2.2,
		1.8,
		1.4,
		1.1,
		0.8,
		0.5,
		0.2,
		0,
		0,
		0,
		0
	};
	private double[] rotationValues4 = {
		0,
		0,
		0,
		0,
		-0.1,
		-0.2,
		-0.3,
		-0.7,
		-1.1,
		-1.5,
		-1.9,
		-2.3,
		-2.7,
		-2.7,
		-2.6,
		-2.2,
		-1.8,
		-1.4,
		-1.1,
		-0.8,
		-0.5,
		-0.2,
		0,
		0,
		0,
		0
	};
	private double[] rotationValues5 = {
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
		0
	};
	private double[] rotationValues6 = {
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
		0
	};
	private double[] rotationValues7 = {
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

	public ColumnStack ()
	{
	}


	double[] getRotationValues ()
	{

		switch (rotationType) {
		case 1:
			return rotationValues1;
		case 2:
			return rotationValues2;
		case 3:
			return rotationValues3;
		case 4:
			return rotationValues4;
		case 5:
			return rotationValues5;
		case 6:
			return rotationValues6;
		case 7:
			return rotationValues7;
		}
		return rotationValues1;
	}

	public void SetCardFaceDirection (int which, bool up)
	{
		CardList [which].Definition.FaceUp = up;
	}

	public bool GetCardFaceDirection (int which)
	{
		return CardList [which].Definition.FaceUp;
	}

	public void SetCardIsClickable (int which, bool itis)
	{
		CardList [which].Definition.Clickable = itis;
	}

	public void SetCardNextCard (int which, int next)
	{
		CardList [which].Definition.NextCard = next;
	}

	public GameCard GetCurrentCard ()
	{
		if (!IsStackEmpty () && CurrentCard != -1) {
			return CardList [CurrentCard];
		}
		Debug.Log ("ERROR IN GetCurrentCard. THIS SHOULD NOT BE REACHED. IsStackEmpty=" + IsStackEmpty ());
		Debug.Log ("#CurrentCard =" + CurrentCard);
		Debug.Log ("#Cards in CardList=" + CardList.Count);
		return null;
	}

	public void SetCurrentCard (int which)
	{
		CurrentCard = which;
	}

	public GameCard GetCard (int which)
	{   
		if (!IsStackEmpty () && CurrentCard != -1) {
			return CardList [which];
		}
		Debug.Log ("ERROR IN GetCard. THIS SHOULD NOT BE REACHED");
		return null;
	}

	public GameCard GetWinningCard (int which)
	{
		if (!IsStackEmpty () && which < CardList.Count) {
			return CardList [which];
		}
		Debug.Log ("ERROR IN GetWinningCard. THIS SHOULD NOT BE REACHED");
		return null;
	}

	public GameCard GetCardUsingOrder (int which)
	{
	
		foreach (GameCard crd in CardList) {
			if (crd.Definition.CardOrder == which) {
				return crd;
			}
		}
		Debug.Log ("ERROR IN GetCardUsingOrder. THIS SHOULD NOT BE REACHED. WHICH=" + which.ToString ());
		return null;
	}


	public GameCard GetTopCard ()
	{
		if (!IsStackEmpty ()) { 
			return CardList [CardList.Count - 1];
		}
		return null;
	}

	public int GetSelectedCardNum ()
	{
		return SelectedCard;
	}

	public int GetSelectedCardValue ()
	{
		return CardList [SelectedCard].Definition.CardValue;
	}

	public int GetSelectedCardColor ()
	{
		return CardList [SelectedCard].Definition.CardColor;
	}

	public GameCard GetSelectedCard ()
	{
		if (!IsStackEmpty () && SelectedCard != -1) {
			return CardList [SelectedCard];
		} else
			return null;
	}

	public void RemoveSelectedCard ()
	{
		if (SelectedCard != -1) {
			CardList.RemoveAt (SelectedCard);

			int count = 0;
			foreach (GameCard c in CardList) {
				c.Definition.CardOrder = count;
				count++;
			}
		} else {
			Debug.Log ("ERROR: Didn't remove card ");
		}
		SelectedCard = -1;
	}

	public GameCard FlipBottomCard ()
	{
		GameCard tempcard = null;

		if (!IsStackEmpty ()) {
			tempcard = GetTopCard ();

			if (!AutoCardTurn) {
				tempcard.SetFlyTarget (tempcard.transform.position, tempcard.transform.position, 0.85f, true);

				tempcard.PlayFlipAnim ();

				return tempcard;
			}
		}
		return null;
	}

	public int CountCards ()
	{
		return CardList.Count;
	}

	public bool CheckIfAce (int which)
	{
		if (CardList [which].Definition.CardValue == 1) {
			return true;			
		}
		return false;	
	}

	 
	public bool IsCardValid (int in_value, int in_color)
	{
		GameCard tmpCard = GetTopCard ();
		bool faceUp = true;

		if (tmpCard != null)
			faceUp = tmpCard.Definition.FaceUp;

//		if (in_value == 13 && CardList.Count == 0) {
		if (CardList.Count == 0) {
//			Debug.Log ("King on empty space");
			return true;
		} else if (IsCardOppositeColor (in_color)) {
			if (in_value == (LastValue - 1) || false) {
				if (faceUp) {
//					Debug.Log ("Valid value and color");
					return true;
				}
			}
		}

//		Debug.Log ("Not valid");
		return false;
	}

	private bool IsCardOppositeColor (int color)
	{
		if ((color == 0 || color == 2) && (LastColor == 1 || LastColor == 3))
			return true;
		if ((color == 1 || color == 3) && (LastColor == 0 || LastColor == 2))
			return true;
		return false;		
	}

	public void RemoveCardFromStack (int which)
	{
		if (!IsStackEmpty ()) {
			CardList.RemoveAt (which);

			int count = 0;
			foreach (GameCard c in CardList) {
				c.Definition.CardOrder = count;
				count++;
			}
		} else {
			Debug.Log ("ERROR: Didn't remove card");
		}
	}

	public bool IsStackEmpty ()
	{
		if (CardList.Count == 0) {
			return true;
		}
		return false;
	}



	public void ResetCurrentCard ()
	{
		CurrentCard = -1;
	}

	public void ResetCardPositions ()
	{
		foreach (GameCard c in CardList) {
			c.transform.localPosition = Vector3.zero;
		}
	}

	public void ResetStuff ()
	{
		SelectedCard = -1;
		LastColor = -1;
		LastValue = -1;
		CurrentCard = -1;
	}


	/***** NAVIGATION KEYBOARD INPUT ******/

	public void MoveTo (int in_card)
	{
		if (!IsStackEmpty ()) {
			if (CurrentCard > 0 && CardList [CurrentCard - 1].Definition.FaceUp == true) {
				CurrentCard = in_card;
			}
		}
	}

	public void MoveUp ()
	{
		if (!IsStackEmpty ()) {
			if (CurrentCard == -1) {
				CurrentCard = CardList.Count - 1;
			}
			if (CurrentCard > 0 && CardList [CurrentCard - 1].Definition.FaceUp == true) {
				CurrentCard--;
			}
		}
	}

	public void MoveDown ()
	{
		if (!IsStackEmpty ()) {
			if (CurrentCard != -1 && CurrentCard != (CardList.Count - 1) && CardList [CurrentCard + 1].Definition.FaceUp == true) {
				CurrentCard++;
			}
			 
		}
	}

	public bool isFaceupCardAbove ()
	{

		if (CurrentCard == 0 || CurrentCard == -1)
			return false;
		if (CardList [CurrentCard - 1].Definition.FaceUp == true)
			return true;

		return false;

	}

	public void SelectCard (bool changeSize)
	{
//		Debug.Log ("I SelectCard. CurrentCard =" + CurrentCard.ToString ());
		 
		if (!IsStackEmpty ()) {
			if (changeSize) {
				UnSelectCard ();
				if (CurrentCard != -1) {
					CardList [CurrentCard].SelectCard ();
				}
			}
			SelectedCard = CurrentCard;
		}
	}

	public void UnSelectCard ()
	{
		if (!IsStackEmpty () && SelectedCard != -1) {
			CardList [SelectedCard].UnSelectCard ();
			SelectedCard = -1;
		}
	}

	public GameCard GetChildCard (int which)
	{

		if (CardList.Count > which) {
			return CardList [which];
		}

		return null;
	}

	public void SelectChild (int which)
	{
		if (!IsStackEmpty ()) {
			CardList [which].SelectCard ();
		}
	}

	public void UnSelectChild (int which)
	{
		if (!IsStackEmpty ()) {
			CardList [which].UnSelectCard ();
		}
	}

	public void RemoveChildCard (int which)
	{
		if (!IsStackEmpty ()) {
			CardList.RemoveAt (which);
		}

		int count = 0;
		foreach (GameCard c in CardList) {
			c.Definition.CardOrder = count;
			count++;
		}
	}

	public void RemoveAllCards ()
	{
		for (int i = 0; i < CountCards (); i++) {
			CardList.RemoveAt (i);
		}
	}

	public bool AnyHiddenCardsLeft ()
	{

		foreach (GameCard c in CardList) {
			if (c.Definition.FaceUp == false) {
				return true;
			}
		}
		return false;
	}


	public string GetDrawCardSound (int whichsound)
	{
		switch (whichsound) {
		case 1:
			return "delaut1";	//delaut1 är för hög
		case 2:
			return "delaut2";
		case 3:
			return "delaut3";
		case 4:
			return "delaut4";
		}
		return "";
	}

	public string GetTurnCardSound (int whichsound)
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
		

	public void SetStack (int in_stack)
	{
		Stack = in_stack;
	}

	public void AddToStack (GameCard in_card)
	{
		
		in_card.transform.parent = MyParent;
//		in_card.transform.localScale = new Vector3 (2.5f, 3.5f, 0.01f); //reset
		LastColor = in_card.Definition.CardColor;
		LastValue = in_card.Definition.CardValue;
		in_card.Definition.Stack = Stack;
		in_card.Definition.CardOrder = CardList.Count - 1;	//CurrentCount; //CardList.Count - 1;
//		CurrentCount++;
		CardList.Add (in_card);
	}

	 
	public bool IsValidBelow(GameCard gc) {
		bool foundGc = false;
		int currVal = gc.Definition.CardValue;
		int currCol = gc.Definition.CardColor;

		foreach (GameCard c in CardList) {
			if (foundGc) {

				if ((currCol == 0 || currCol == 2) && (c.Definition.CardColor == 0 || c.Definition.CardColor == 2))
					return false;
				if ((currCol == 1 || currCol == 3) && (c.Definition.CardColor == 1 || c.Definition.CardColor == 3))
					return false;
				currCol = c.Definition.CardColor;

				currVal--;
				if (c.Definition.CardValue != currVal)
					return false;
			}

			if (c == gc) {
				foundGc = true;
			}
		}

		return foundGc;
	}
		

	public void InitGameObjects ()
	{
		thisColumn = GameObject.Find ("ColumnStack" + Stack.ToString ()).GetComponent<ColumnStack> ();
		thisGame = GameObject.Find ("KlondikeGame").GetComponent<KlondikeGame> ();
	}

	public void OrganizeStack (bool withFlip, bool isTouch, bool doPositioning, bool animate)
	{
		GameCard tempcard = null;
		float x;
		float y;
		float z = -1.0f;// = ParamStartStackCardsZ;
		int count = 0;
		bool lastFaceup = false;
		float yPosAccum = 0;

		CardList.RemoveRange (0, CardList.Count);	//Clean CardList

//		Debug.Log ("COLUMN: OrganizeStack. Cards in CardList=" + CardList.Count.ToString ());

		//Recreate list
		foreach (Transform child in thisColumn.transform) {
			tempcard = child.gameObject.GetComponentInParent<GameCard> ();

			tempcard.Definition.Stack = Stack;
			tempcard.Definition.CardOrder = count;

			if (tempcard.Definition.FaceUp) {
				tempcard.Definition.Clickable = true;
			} else {
				tempcard.Definition.Clickable = false;
			}

			tempcard.targetStack = -1;

			//Add to local list
			CardList.Add (tempcard);

			count++;
		}

		//Positions
		count = 0;

		tempcard = null;
		foreach (GameCard c in CardList) {

			if (!isTouch) {
//				c.transform.localScale = new Vector3 (2.5f, 3.5f, c.transform.localScale.z);
			}
			if (doPositioning) {	//Only target stack needs re-positioning

				x = 0;
//				y = -((count * ParamYBetweenHiddenCards) + Random.Range (0, 0.15f));
				y = -yPosAccum;
				yPosAccum += ParamYBetweenHiddenCards + Random.Range (-0.03f, 0.17f);

				x = Random.Range (-0.05f, 0.05f);

				if (c.Definition.FaceUp == true) {
					c.Definition.Clickable = true;
					if (lastFaceup == true) {
						y = tempcard.transform.localPosition.y - Random.Range (0.42f, 0.42f); //Utgå från föreg. kort
					}
					if (firstTimeEver) {
						c.transform.rotation = Quaternion.Euler (new Vector3 (0, 0.0f, (float)-getRotationValues () [count]));
//						c.PlayIdleAnim ();
					}
				} else {
					c.Definition.Clickable = false;
					if (firstTimeEver) {
						c.transform.rotation = Quaternion.Euler (new Vector3 (0, 0.0f, (float)getRotationValues () [count]));
					}
				}

				if (tempcard != null) {
					z = tempcard.transform.localPosition.z - 0.1f;
				} else {
					z = -0.1f;
				}


				//Check if we need to set other position
				if (count != CardList.Count
				    && System.Math.Round (c.transform.localPosition.x, 2) == System.Math.Round (x, 2)
				    && System.Math.Round (c.transform.localPosition.y, 2) == System.Math.Round (y, 2)
				    && System.Math.Round (c.transform.localPosition.z, 2) == System.Math.Round (z, 2)) {
//					Debug.Log ("Don't need to position this card. Card val = " + c.Definition.CardValue);
				} else {
//					Debug.Log ("Position this card. Card val = " + c.Definition.CardValue);
					if (animate == false) {
						if (firstTimeEver) {
							c.transform.localPosition = new Vector3 (x, y, z);
						}

					} else {
						if (lastFaceup) {
							iTween.MoveTo (c.gameObject, iTween.Hash ("position", new Vector3 (x, y, z), "islocal", true, "time", 0.5f));
						}
					}
				}
			}

			c.Definition.PrevCard = count - 1;
			c.Definition.NextCard = count + 1;

			c.sourceStack = -1;
			c.targetStack = -1;

			lastFaceup = c.Definition.FaceUp;
			tempcard = c;
			count++;
		}


		//Only last object 
		if (tempcard != null) {

			if ((withFlip && !lastFaceup) || (withFlip && firstTimeEver)) {	//Animate a flip

				firstTimeEver = false;

				tempcard.targetStack = -1;
				if (AutoCardTurn || !isTouch) {
					//	tempcard.SetFlyTarget (tempcard.transform.position, tempcard.transform.position, FlipFlyTime, false);	
				}
			} else {
//				tempcard.transform.rotation = Quaternion.Euler (new Vector3 (0, 0.0f, tempcard.transform.rotation.z)); 
				//			tempcard.transform.rotation = Quaternion.Euler(new Vector3(0,180,180)); 
			}

			LastColor = tempcard.Definition.CardColor;
			LastValue = tempcard.Definition.CardValue;
			CardList [CardList.Count - 1].Definition.Stack = Stack;
			CardList [CardList.Count - 1].Definition.NextCard = -1;
			CardList [CardList.Count - 1].Definition.Clickable = true;
		}

		SelectedCard = -1;
	}

	public void OrganizeStack (bool withFlip, bool isTouch, bool doPositioning)
	{
		OrganizeStack (withFlip, isTouch, doPositioning, false);
	}
		
	//Denna används bara efter Reset (starta om spel). Bug finns fortfarande eftersom något eller några av korten inte vänds rätt (rotation) av någon anledning.
	public void OrganizeStackRestart ()
	{
		GameCard tempcard = null;
		float x;
		float y;
		float z = -1.0f;// = ParamStartStackCardsZ;
		int count = 0;
		bool lastFaceup = false;

	
//		Debug.Log ("COLUMN: OrganizeStackRestart. Cards in CardList=" + CardList.Count.ToString ());
		 
		//Positions
		count = 0;
		foreach (GameCard c in CardList) {

			x = 0;
			y = -(count) * (ParamYBetweenHiddenCards); 

			if (c.Definition.FaceUp == true) {
				c.Definition.Clickable = true;
				c.transform.rotation = Quaternion.Euler (new Vector3 (0, 0f, (float)-getRotationValues () [count]));
				 
				if (lastFaceup == true) {
					y = tempcard.transform.localPosition.y - 0.45f;
				}
			} else {
				c.Definition.Clickable = false;
				c.transform.rotation = Quaternion.Euler (new Vector3 (0, 0f, (float)getRotationValues () [count])); 
			}
				
			if (tempcard != null) {
				z = tempcard.transform.localPosition.z - 0.08f;
			} else {
				z = -0.08f;
			}

			c.transform.localPosition = new Vector3 (x, y, z);

			if (c.Definition.FaceUp) {
				c.Definition.Clickable = true;
				c.SetFlyTarget (c.transform.position, c.transform.position, FlipFlyTime, true);	
			} else {
				c.Definition.Clickable = false;
				c.SetFlyTarget (c.transform.position, c.transform.position, 0f, false);	
			}
			 
			c.targetStack = -1;
			c.Definition.Stack = Stack;
			c.Definition.CardOrder = count;
			c.Definition.PrevCard = count - 1;
			c.Definition.NextCard = count + 1;

			c.sourceStack = -1;
			c.targetStack = -1;
			lastFaceup = c.Definition.FaceUp;
			tempcard = c;
			count++;
		}


		//Only last object 
		if (tempcard != null) {
			thisGame.PlayCardSound (GetTurnCardSound (Random.Range (1, 3)), 0.3f, Stack);
			
			tempcard.targetStack = -1;


			LastColor = tempcard.Definition.CardColor;
			LastValue = tempcard.Definition.CardValue;
			CardList [CardList.Count - 1].Definition.Stack = Stack;
			CardList [CardList.Count - 1].Definition.NextCard = -1;
			CardList [CardList.Count - 1].Definition.Clickable = true;
		}

		SelectedCard = -1;
	}


	public void Reset() {
		firstTimeEver = true;
		CardList = new List<GameCard> ();
		SelectedCard = -1;
		LastColor = -1;
		LastValue = -1;
		CurrentCard = -1;
	}

}