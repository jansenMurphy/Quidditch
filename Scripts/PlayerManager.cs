using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

[RequireComponent(typeof(Camera))]
public class PlayerManager : MonoBehaviour {

	//Camera cam;
	public int team = 0, position = 0;
	/*
		team 1==Gryffindor, 2==Ravenclaw 3==Hufflepuff 4==Slytherin
		position 1==seeker, 2==Keeper, 3==Chaser, 4==beater
	*/

	void Start(){
		//cam = GetComponent<Camera> ();
		if (!VRDevice.isPresent)
			Debug.Log ("NO DEVICE PRESENT");
	}
}
