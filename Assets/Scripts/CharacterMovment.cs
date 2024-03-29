﻿using UnityEngine;
using System.Collections;

public class CharacterMovment : MonoBehaviour {
	public float speed = 0F;
	public float jumpSpeed = 0F;
	public float moveTimeToRotate =0f;
	bool hasJumped = false;
	float jumpTime = 0f;
	float moveStartTime =0f;
	bool rotated = false;


	Vector3 jumpVector = new Vector3(0F,0F,0F);
	Vector3 moveDirection = new Vector3(0F,0F,0F);

	public Transform target = null;
	public float cameraDistance = 0F;
	public float cameraHeight = 0F;
	public float cameraSpeed = 5f;
	private Vector3 cameraDirection;
	private Vector3 cameraPosition;



	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < 1)
		{
			transform.position = new Vector3(transform.position.x,1,transform.position.z);
		}


		//parts lifted from unity reference
		CharacterController controller = GetComponent<CharacterController>();

		moveDirection = new Vector3(0F,0f,0F);
		//setting vector for how object will move this frame based on input
		if(Input.GetKey(KeyCode.W)){
			//controller.Move ( transform.forward * Time.deltaTime);
			moveDirection= moveDirection + ( transform.forward * Time.deltaTime);
		}
		
		//starts countdown untill camera changes and gets ready to rotate
		if(Input.GetKeyDown(KeyCode.S)){
			moveStartTime=Time.time;
			rotated=false;
		}

		if(Input.GetKey(KeyCode.S)){
			if((Time.time-moveStartTime)>moveTimeToRotate){
				transform.Rotate(Vector3.up, 180);
				//player rotation only effects the camera
				moveStartTime=Time.time;
				rotated=true;
			}
			if (rotated){
				//since forward is the direction player is going button now pushes player forward
				moveDirection= moveDirection + ( transform.forward * Time.deltaTime);
				moveStartTime=Time.time;
			}
			else{
				moveDirection= moveDirection - ( transform.forward * Time.deltaTime);
			}
		}
		
		//repeat for other keys
		if(Input.GetKeyDown(KeyCode.D)){
			moveStartTime=Time.time;
			rotated=false;
		}


		if(Input.GetKey(KeyCode.D)){
			if((Time.time-moveStartTime)>moveTimeToRotate){
				transform.Rotate(Vector3.up, 90);
				moveStartTime=Time.time;
				rotated=true;
			}
			if (rotated){
				moveDirection= moveDirection + ( transform.forward * Time.deltaTime);
				moveStartTime=Time.time;
			}
			else{
				moveDirection= moveDirection + ( transform.right * Time.deltaTime);
			}
		}
		
		
		if(Input.GetKeyDown(KeyCode.A)){
			moveStartTime=Time.time;
			rotated=false;
		}

		if(Input.GetKey(KeyCode.A)){
			if((Time.time-moveStartTime)>moveTimeToRotate){
				transform.Rotate(Vector3.up, -90);
				moveStartTime=Time.time;
				rotated = true;
			}
			if (rotated){
				moveDirection= moveDirection + ( transform.forward * Time.deltaTime);
				moveStartTime=Time.time;
			}
			else{
				moveDirection= moveDirection - ( transform.right * Time.deltaTime);
			}
		}


		 



		if (controller.isGrounded) {
			jumpVector = new Vector3(0F,0F,0F);
			hasJumped = false;
			//space starts jump
			if(Input.GetKey(KeyCode.Space)){
				jumpVector = new Vector3(0F,jumpSpeed,0F);
				controller.Move ( jumpVector *Time.deltaTime);
				hasJumped= true;
				jumpTime= Time.time;
			}
			//add speed to the whole thing
			moveDirection.z *= speed;
			moveDirection.x *= speed;
		} else {
			//give the player a little air control
			moveDirection.z *= speed*0.75f;
			moveDirection.x *= speed*0.75f;
		}


		if ((Time.time - jumpTime) > 5) {
			//occaisionally a jump will hit a surface but isgrounded() won't trigger causeing player to fly away
			//This timer is a safe guard against that 
			hasJumped = false;
			jumpVector = new Vector3(0F,0F,0F);
		}

		if (hasJumped) {
			//adds height to diminishing degrees
			controller.Move (jumpVector * Time.deltaTime);
			jumpVector.y = jumpVector.y-(5 * Time.deltaTime);
		}

		//
		//Camera Control was done with the help of users Navigatron and asafsitner at answers.unity3d.com
		// Keeps camera a set distance behind player
		cameraPosition = transform.position - (transform.forward * cameraDistance);
		cameraPosition.y += cameraHeight;
		target.position = Vector3.MoveTowards(target.position, cameraPosition, cameraSpeed*Time.deltaTime);
		// finds vector between camera and player
		cameraDirection = (transform.position - target.position).normalized;
		//puts vector into the format that the rotation variable is and turns camera towards player.
		target.rotation =  Quaternion.LookRotation(cameraDirection);

		controller.Move ( moveDirection + jumpVector ); 
		controller.Move ( moveDirection + (Physics.gravity *Time.deltaTime) ); 
		Debug.Log (moveDirection.ToString ());

		
	
	
	}
}
