using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Direction currentDir;
	// Vector2 input;
	// bool isMoving = false;
	// Vector3 startPos;
	// Vector3 endPos;
	// float t;
	// private Animator anim;

	// public float walkSpeed = 1f;

	// void Start(){
	// 	anim = GetComponentInChildren<Animator>();
	// }
	
	// // Update is called once per frame
	// void Update () {
	// 	if(!isMoving){
	// 		input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	// 		if(Mathf.Abs(input.x) > Mathf.Abs(input.y))
	// 			input.y = 0;
	// 		else
	// 			input.x = 0;

	// 		if(input != Vector2.zero){
	// 			if(input.x < 0){
	// 				currentDir = Direction.West;
	// 			}
	// 			if(input.x > 0){
	// 				currentDir = Direction.East;
	// 			}
	// 			if(input.y < 0){
	// 				currentDir = Direction.South;
	// 			}
	// 			if(input.y > 0){
	// 				currentDir = Direction.North;
	// 			}

	// 			switch(currentDir){
	// 				case Direction.North:
	// 					anim.Play("NorthAnimation");
	// 					break;
	// 				case Direction.East:
	// 					anim.Play("EastAnimation");
	// 					break;
	// 				case Direction.South:
	// 					anim.Play("SouthAnimation");
	// 					break;
	// 				case Direction.West:
	// 					anim.Play("WestAnimation");
	// 					break;
	// 			}


	// 			StartCoroutine(Move(transform));
	// 		}else{
	// 			anim.Play("Idle");
	// 		}
	// 	}
	// }

	// public IEnumerator Move(Transform entity){
	// 	isMoving = true;
	// 	startPos = entity.position;
	// 	t = 0;

	// 	endPos = new Vector3(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y), startPos.z);

	// 	while (t < 1f){
	// 		t += Time.deltaTime * walkSpeed;
	// 		entity.position = Vector3.Lerp(startPos, endPos, t);
	// 		yield return null;
	// 	}
	// 	isMoving = false;
	// 	yield return 0;
	// }

	public float speed = 2.0f;
	private Vector3 target;
	private float diffX;
	private float diffY;
	private Animator anim;

	void Start () {
		target = transform.position;
		anim = GetComponentInChildren<Animator>();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = transform.position.z;

			if(target.x * transform.position.x >= 0){
				diffX = Mathf.Abs(Mathf.Abs(target.x) - Mathf.Abs(transform.position.x));
			}else{
				diffX = Mathf.Abs(target.x) + Mathf.Abs(transform.position.x);
			}
			if(target.y * transform.position.y >= 0){
				diffY = Mathf.Abs(Mathf.Abs(target.y) - Mathf.Abs(transform.position.y));
			}else{
				diffY = Mathf.Abs(target.y) + Mathf.Abs(transform.position.y);
			}
		}
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

		if (diffX >= diffY) {
			if(target.x < transform.position.x) {
				anim.Play("WestAnimation");
			}else if(target.x > transform.position.x) {
				anim.Play("EastAnimation");
			};
		} else {
			if(target.y < transform.position.y) {
				anim.Play("SouthAnimation");
			}else if(target.y > transform.position.y) {
				anim.Play("NorthAnimation");
			};
		}

		if (target.x == transform.position.x && target.y == transform.position.y){
			anim.Play("Idle");
		}
	}
}

// enum Direction{
// 	North,
// 	East,
// 	South,
// 	West
// }
