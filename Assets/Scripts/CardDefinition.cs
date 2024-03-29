using UnityEngine;
using System.Collections;

public class CardDefinition : MonoBehaviour
{
	public CardDef Data;
}

[System.Serializable]
public class CardDef
{
	public string Text;
	public string Symbol; 
	public int CardValue;
	public int CardColor;
		
	public int Stack;
	public int CardOrder;
	public bool FaceUp;
	public bool Clickable;
	public int NextCard;
	public int PrevCard;

	public CardDef(string text, string symbol, int cardvalue, int cardcolor)
	{
		Text = text;
		Symbol = symbol;
		CardValue = cardvalue;
		CardColor = cardcolor;
	}
}