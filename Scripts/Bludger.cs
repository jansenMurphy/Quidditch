using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bludger : SmartBall {

	Vector3 intendedVelocity;
	public float slowness =1;

	float timeUntilChangeDirection = 0f;
	[SerializeField]
	float timeAfterRandomMove = 1f, timeAfterhit = 4f, timeAfterThrown = 5f;
	public float addedForceMultiplierMin =.75f, addedForceMultiplierMax =2.2f;
	Rigidbody rb;

	void Start(){
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		timeUntilChangeDirection -= Time.deltaTime;
		if (!isCaught) {
			Vector3 tgt = locatePlayers (0);
			float dist;
			if (dist = Vector3.Distance (tgt, this.transform.position) > .01f) {
				mainAction (tgt, dist);
			} else {
				flyRandom ();
			}

			rb.AddForce ((intendedVelocity - rb.velocity) / (Time.fixedDeltaTime * slowness));
		}
	}

	protected override void flyRandom (){
		if (timeUntilChangeDirection <= 0) {
			intendedVelocity = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized * Random.Range(addedForceMultiplierMin, addedForceMultiplierMax);
			timeUntilChangeDirection = timeAfterRandomMove;
		}

	}

	void OnCollision(Collision col){
		timeUntilChangeDirection = timeAfterhit;
		if(col.gameObject.tag == "Player"){
			Debug.Log("Hit player");//col.gameObject.GetComponent<PlayerManager>().isHit(Vector3 rb.velocity);
			intendedVelocity = (col.contacts[0].point - col.transform.position).normalized * Random.Range(addedForceMultiplierMin, addedForceMultiplierMax)*2;
		}
	}

	protected override void mainAction(Vector3 vec, float dist){
		if (timeUntilChangeDirection <= 0) {
			intendedVelocity = (vec+new Vector3(Random.Range(-dist/2f, dist/2f), Random.Range(-dist/2f,dist/2f),Random.Range(-dist/2f,dist/2f)) -
				transform.position)* Random.Range(addedForceMultiplierMin, addedForceMultiplierMax);
					
			timeUntilChangeDirection = timeAfterRandomMove;
		}
	}
		
	public override void Thrown(){
		timeUntilChangeDirection = timeAfterThrown;
		timeUntilFocusEitherTeam = timeFocusedOnATeam;
		intendedVelocity = Vector3.zero; //NEEDS TO BE ACTUAL CODE HERE
	}
}
