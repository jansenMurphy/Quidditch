using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	public GameObject currentMenu, MainMenu, Lobby;

	public void toMainMenu(){
		if(currentMenu != null)
			currentMenu.SetActive(false);
		currentMenu = MainMenu;
		currentMenu.SetActive(true);
	}

	public void toLobby(){
		if(currentMenu != null)
			currentMenu.SetActive(false);
		currentMenu = Lobby;
		currentMenu.SetActive(true);
	}
}
