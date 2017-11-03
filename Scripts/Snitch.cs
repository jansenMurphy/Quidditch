using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Snitch : SmartBall {

	float timeUntilChangeDirection = 0f;
	[SerializeField]
	float timeAfterRandomMove = 1, timeAfterhit = 4;
	public float addedForceMultiplierMin =.75f, addedForceMultiplierMax =2.2f;
	Rigidbody rb;

	void Start(){
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		timeUntilChangeDirection -= Time.deltaTime;
		if (!isCaught) {
			Vector3 tgt = locatePlayers ();
			if (Vector3.Distance (tgt, this.transform.position) > .01) {
				mainAction (tgt);
			}
		}
	}

	protected override void flyRandom (){
		if (timeUntilChangeDirection <= 0) {
			rb.velocity = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized * Random.Range(addedForceMultiplierMin, addedForceMultiplierMax);
			timeUntilChangeDirection = timeAfterRandomMove;
		}

	}

	protected override void mainAction(Vector3 vec){
		float dist = Vector3.Distance (vec, transform.position);
		if (dist < .01) {
			flyRandom ();
		}

		if (timeUntilChangeDirection <= 0) {
			rb.velocity = (vec+new Vector3(Random.Range(-dist/3f, dist/3f), Random.Range(-dist/3f,dist/3f),Random.Range(-dist/3f,dist/3f)) -
				transform.position)* Random.Range(addedForceMultiplierMin, addedForceMultiplierMax);

			timeUntilChangeDirection = timeAfterRandomMove;
		}
	}


	public override void Caught(){
		GameManager.endGame ();
	}
}
