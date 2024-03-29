using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGame : MonoBehaviour {

	public GameObject stacks;
	public GameObject mainMenu;
	public KlondikeGame game;
	public GameObject zoom;
	public GameObject hamburgerMenu;
	// Use this for initialization
	void Start () {
		
	}

	public void StartGame() {
		stacks.SetActive (true);
		mainMenu.SetActive (false);
		game.DoStart();
		zoom.SetActive (true);
		hamburgerMenu.SetActive (true);
	}

	public void ShowMainMenu() {
		stacks.SetActive (false);
		mainMenu.SetActive (true);
		hamburgerMenu.SetActive (false);
	}

}
