using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;


public class MatchListItem : MonoBehaviour {

	[SerializeField]
	Text txt1;
	MatchInfoSnapshot snapshot;

	void Start(){
		txt1 = GetComponent<Text> ();
		GetComponent<Button> ().onClick.AddListener (taskOnClick);
	}

	public void CreateOrModify (LobbyManager _nLMan, MatchInfoSnapshot _snapshot) {
		this.snapshot = _snapshot;
		txt1.text = _snapshot.name;
	}

	void taskOnClick(){
		LobbyManager.lobbymanagerSingleton.MMJoinMatch (snapshot);
	}
}
