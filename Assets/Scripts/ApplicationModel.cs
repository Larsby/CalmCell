using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel : MonoBehaviour {

	public static bool bShowInitialButtons = true;

	public const int NOF_SONGS = 10;
	public static int[] usedSongs = new int[NOF_SONGS] { 0,0,0,0,0,0,0,0,0,0 };
	public static int songCounter = 0;
	public static string[] songNames = { "music5", "Klondike-Zen4416", "Ambient_Beauty", "Deep_Meditation", "Ethereal_Bliss", "First_Steps", "Resonance", "Tree_of_Life-Main_Theme", "Tree_of_Life-Next_journey1", "Tree_of_Life-Next_journey2" };
}
