using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartBall : MonoBehaviour {

	public int minPlayerDistance = 20;
	int focusTeam = 0;
	protected float timeUntilFocusEitherTeam = 0f;
	public float timeFocusedOnATeam = 15f;

	public List<PlayerManager> players = new List<PlayerManager> ();

	protected bool isCaught = false;

	protected virtual Vector3 locatePlayers(int team){
		float distance = minPlayerDistance + 20, td;

		if (players.Count <= 0)
			return this.transform.position; // Needs to run around randomly

		int playerTargeted=players.Count;

		for (int i = 0; i < players.Count; i++) {
			if ((td = Vector3.Distance (players[i].position, transform.position)) < distance) {
				if (team == 0 || team == players [i].team) {
					distance = td;
					playerTargeted = i;
				}
			}
		}

		if (distance < minPlayerDistance){
			return players [playerTargeted].position;
		}else{
			return this.transform.position;
		}
	}

	protected abstract void flyRandom ();
	protected abstract void mainAction (Vector3 target, float dist);

	public abstract void Caught ();
	public abstract void Thrown ();
}
