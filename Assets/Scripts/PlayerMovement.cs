using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public float speed = 2.0f;
	private Vector3 target;
	private float diffX;
	private float diffY;
	private Animator anim;
	private string selectName;
	private string hitName;
	public GameObject housePanel;
	public GameObject dialogueBox;
	public GameObject pickUpPanel;
	public GameObject itemBtn;
	public GameObject sceneLoader;
	public GameObject[] itemSpots;
	private List<string> itemCollected;

	void Start () {
		target = transform.position;
		anim = GetComponentInChildren<Animator>();
		itemCollected = new List<string>();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) 
			&& !housePanel.active 
			&& !dialogueBox.active
			&& !pickUpPanel.active
			&& Input.mousePosition.x <= (itemBtn.transform.position.x - 20)) {
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
			if(other.gameObject.tag == "House"){
				housePanel.SetActive(true);
			}
			if(other.gameObject.tag == "Exit"){
				housePanel.SetActive(true);
			}
			if(other.gameObject.tag == "NPC"){
				other.gameObject.GetComponent<DialogueTrigger>().triggerDialogue();
				hitName = "";
			}
			if(other.gameObject.tag == "PickUp"){
				pickUpPanel.SetActive(true);
				pickUpPanel.GetComponentInChildren<Text>().text = "You found a " + hitName + "! Pick it up?";
			}
		}
	}

	public void enterHouse(){
		sceneLoader.GetComponent<SceneLoader>().enterHouse();
	}

	public void exitHouse(){
		sceneLoader.GetComponent<SceneLoader>().exitHouse();
	}

	public void closeHousePanel(){
		housePanel.SetActive(false);
		hitName = "";
	}

	public void pickUpItem(){
		for(int i = 0; i < itemSpots.Length; i++){
			if(itemSpots[i].GetComponent<Image>().sprite == null){
				itemCollected.Add(hitName);
				itemSpots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(hitName);
				GameObject.Find(hitName).GetComponent<DialogueTrigger>().triggerDialogue();
				GameObject.Find(hitName).SetActive(false);
				break;
			}
		}
		closePickUpPanel();
		hitName = "";
	}

	public void closePickUpPanel(){
		pickUpPanel.SetActive(false);
		hitName = "";
	}
}
