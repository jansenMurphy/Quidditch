using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OverriddenNetworkDiscovery : NetworkDiscovery {

	/*
	void Start(){
		broadcastData = 
	}
	*/

	public override void OnReceivedBroadcast(string address, string data){
		LobbyManager.singleton.networkAddress = address;
		LobbyManager.singleton.StartClient ();
	}

}
