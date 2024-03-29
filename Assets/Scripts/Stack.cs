using System;


public interface Stack
{

	void AddToStack (GameCard in_card, bool isTouch);

	GameCard GetCard (int which);

	GameCard GetTopCard ();

}

