using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public float speed = 2.0f;
	private Vector3 target;
	private float diffX;
	private float diffY;
	private Animator anim;
	private string selectName;
	private string hitName;
	public GameObject housePanel;

	void Start () {
		target = transform.position;
		anim = GetComponentInChildren<Animator>();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && !housePanel.active) {
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
			
			Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                          Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);
			if (hit) {
				hitName = hit.transform.gameObject.name;
				print(hit.transform.gameObject.name);
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

	void OnCollisionStay2D(Collision2D other){
		if(other.gameObject.name == hitName){
			target.x = transform.position.x;
			target.y = transform.position.y;
			hitName = "";
			if(other.gameObject.tag == "House"){
				housePanel.SetActive(true);
			}
			if(other.gameObject.tag == "Exit"){
				housePanel.SetActive(true);
			}
		}
	}

	public void closePanel(){
		housePanel.SetActive(false);
	}
}
