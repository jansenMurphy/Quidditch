using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Vector3 broomstickVector;
	Rigidbody rb;
	float interpConstant =1, maxForward=3,maxBack=.5,maxUp=1, maxRight=1, maxLeft=1, maxDown=1;

	void Start(){
		rb = GetComponentInParent<Rigidbody> ();
	}

	void Update(){
		broomstickVector.Normalize ();
		broomstickVector = 
			new Vector3(Mathf.Lerp(maxLeft, maxRight, Input.GetAxis("LeftRight")),
				Mathf.Lerp(maxUp, maxDown, Input.GetAxis("UpDown")),
				Mathf.Lerp(maxForward, maxBack, Input.GetAxis("ForwardBack")));
		rb.AddForce (
			Mathf.Lerp (0, transform.TransformDirection(broomstickVector).x - rb.velocity.x, Time.deltaTime * interpConstant),
			Mathf.Lerp (0, transform.TransformDirection(broomstickVector).y - rb.velocity.y, Time.deltaTime * interpConstant),
			Mathf.Lerp (0, transform.TransformDirection(broomstickVector).z - rb.velocity.z, Time.deltaTime * interpConstant));
		
	}
}
