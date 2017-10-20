using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager {

	// Code is derived from Imperium42 Unity 5.5 Unet tutorial on youtube
	NetworkDiscovery netDisc;
	public GameObject matchListItem;
	public GameObject matchListWindow;
	public GameObject addLocalPlayerButton;
	public GameObject redTeamZone, blueTeamZone;
	public Button startGame;
	protected ulong _currentMatchID;
	public static LobbyManager lobbymanagerSingleton;


	private int _playerNumber;
	public List <MyNetworkLobbyplayer> MNLP = new List<MyNetworkLobbyplayer>();

	//public Text mapText;

	void Start(){
		netDisc = GetComponent<NetworkDiscovery> ();
		netDisc.Initialize ();

		if(lobbymanagerSingleton == null)
			lobbymanagerSingleton = this;
		//mapText.text = this.playScene;
	}

	public void MMStart(){
		Debug.Log ("MMStart");
		this.StartMatchMaker ();
	}

	public void MMListMatches(){
		Debug.Log ("MMListMatches");
		this.matchMaker.ListMatches (0, 5, "", true, 0, 0, OnMatchList);
	}

	public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList){ 

		Debug.Log ("Matches Listed");
		base.OnMatchList(success, extendedInfo, matchList);

		if (!success)
			Debug.Log ("Failed to list matches " + extendedInfo);
		else {
			for (int i = 0; matchList.Count > i; i++) {
				GameObject curItem;
				if (matchListWindow.transform.childCount > i) {
					curItem = matchListWindow.transform.GetChild (i).gameObject;
				} else {
					curItem = GameObject.Instantiate (matchListItem);
					curItem.transform.SetParent (matchListWindow.transform);
				}
				curItem.GetComponent<MatchListItem> ().CreateOrModify (this, matchList [i]);
			}
			while (matchList.Count > matchListWindow.transform.childCount) {
				GameObject.Destroy(matchListWindow.transform.GetChild (matchListWindow.transform.childCount - 1).gameObject);
			}
		}
	}

	public void MMJoinLAN(){
		StartHost ();
		netDisc.StartAsServer ();
	}

	public void MMHostLAN(){
		netDisc.StartAsClient ();
	}

	public void stopLanBroadcast(){
		netDisc.StopBroadcast ();
	}

	public void MMJoinMatch(MatchInfoSnapshot matchListIndex){
		Debug.Log ("MMJoinMatch");
		try{
		this.matchMaker.JoinMatch (matchListIndex.networkId, "", "", Network.player.ipAddress, 0, 0, OnMatchJoined);
		} catch (System.Exception e){
			Debug.LogError (e);
			MMListMatches ();
		}
	}

	public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo){ 

		Debug.Log ("Match join attempt");
		base.OnMatchJoined(success, extendedInfo, matchInfo);

		if (!success)
			Debug.Log ("Failed to join matche " + extendedInfo);
		else {
			Debug.Log ("Joined Match " + matchInfo.networkId);
			//TODO 
		}
	}

	public void MMCreateMatch(){
		Debug.Log ("MMCreateMatch");
		this.matchMaker.CreateMatch ("HAHA", 4, true, "", "", "", 0, 0, this.OnMatchCreate);
	}

	public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo){
		base.OnMatchCreate (success, extendedInfo, matchInfo);
		if (success) {
			_currentMatchID = (System.UInt64)matchInfo.networkId;
			Debug.Log ("Created match" + matchInfo.networkId);
		} else
			Debug.Log ("OOPS" + matchInfo);
	}

	public void MMGoToLevel(){
		foreach (MyNetworkLobbyplayer player in MNLP){
			player.transform.SetParent (null);
			DontDestroyOnLoad (player);
		}
		ServerChangeScene (playScene);
	}

	public void OnClientNotReady(){
		startGame.interactable = false;
	}

		
	public override void OnLobbyServerPlayersReady()
	{
		bool allready = true;
		for(int i = 0; i < lobbySlots.Length; ++i)
		{
			if(lobbySlots[i] != null)
				allready &= lobbySlots[i].readyToBegin;
		}

		if(allready)
			startGame.interactable = true;
	}

	public void MMExitMatch(){
		this.StopMatchMaker ();
	}

	public void MMAddLocalPlayer(){
		TryToAddPlayer ();
	}

	public void MMPlayLocal(){
		StartHost ();
	}

	void OnApplicationQuit(){
		MMStopEverything ();
	}
		
	public void MMStopEverything(){
		if (this.matchMaker != null) {
			matchMaker.DestroyMatch ((UnityEngine.Networking.Types.NetworkID)_currentMatchID, 0, OnMatchDestroy);
			this.StopMatchMaker ();
		}
		this.StopHost ();
	}
	public void OnMatchDestroy(bool success, string extendedInfo){
		//Stuff?
	}

	public void MMChangeLevel(string levelName){
		this.playScene = levelName;
		//mapText.text = levelName;
	}

	//This is ripped from the Lobby Manager asset unity made

	public void AddPlayer(MyNetworkLobbyplayer player){
		if (MNLP.Contains (player))
			return;
		if (MNLP.Count >= maxPlayers) {
			Debug.LogError ("Couldn't join full server");
			MMStopEverything ();
			return;
		}
		MNLP.Add (player);
		player.transform.SetParent (redTeamZone.transform, false);
		player.transform.SetAsLastSibling ();
		addLocalPlayerButton.SetActive (ClientScene.localPlayers.Count < maxPlayersPerConnection && MNLP.Count < maxPlayers ? true : false);
	}

	public void RemovePlayer(MyNetworkLobbyplayer player){
		if (MNLP.Contains (player)) {
			MNLP.Remove (player);
		}
		if (player != null) {
			player.transform.SetParent (null);
		}
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer){
		base.OnLobbyServerSceneLoadedForPlayer (lobbyPlayer, gamePlayer);
		MyNetworkLobbyplayer llm = lobbyPlayer.GetComponent<MyNetworkLobbyplayer> ();
		gamePlayer.name = llm.name;
		GameManager.players.Add(gamePlayer.GetComponent<PlayerManager> ());
		Destroy (lobbyPlayer);
		return true;
	}

	public override void OnClientSceneChanged(NetworkConnection conn){
		//Do code here that needs to be run on every client machine but not on every player
		base.OnClientSceneChanged (conn);
	}
}
