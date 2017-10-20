using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


//Most of this code is derived from the free lobby on the unity asset store.
public class MyNetworkLobbyplayer : NetworkLobbyPlayer {

	[SerializeField]
	Button readyButton;


	public override void OnClientEnterLobby(){
		base.OnClientEnterLobby ();

		LobbyManager.lobbymanagerSingleton.AddPlayer (this);
		LobbyManager.lobbymanagerSingleton.OnClientNotReady ();

		if (isLocalPlayer) {
			SetUpLocalPlayer ();
		} else {
			SetUpNonLocalPlayer ();
		}
	}


	public override void OnStartAuthority(){
		base.OnStartAuthority();

		//if we return from a game, color of text can still be the one for "Ready"
		//readyButton.transform.GetComponent<Text>().color = Color.white;

		SetUpLocalPlayer();
		Debug.Log ("Local Player Joined");
	}

	void SetUpLocalPlayer(){
		readyButton.interactable = true;
	}
	void SetUpNonLocalPlayer(){
		readyButton.transform.GetComponent<Text>().text = "...";
		readyButton.interactable = false;

		OnClientReady(false);
	}

	public override void OnClientReady(bool readyState){
		if (this.readyToBegin){
			readyButton.transform.GetComponent<Text>().text = "READY";
		}else{
			this.readyButton.transform.GetComponent<Text>().text = isLocalPlayer ? "JOIN" : "WAITING";
		}
	}
	public void ReadyButtonClick(){
		readyToBegin = !readyToBegin;
		if (readyToBegin) {
			SendReadyToBeginMessage ();
			LobbyManager.lobbymanagerSingleton.OnLobbyServerPlayersReady ();
		} else {
			LobbyManager.lobbymanagerSingleton.OnClientNotReady ();
		}

		OnClientReady (readyToBegin);
	}
		
	/*
	public override void OnClientExitLobby(){
		LobbyManager.lobbymanagerSingleton.RemovePlayer (this);
	}
	*/
	void OnDestroy(){
		LobbyManager.lobbymanagerSingleton.RemovePlayer(this);
	}
}
