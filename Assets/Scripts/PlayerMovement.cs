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
	public Text dialogueName;
	public Text dialogueText;
	public GameObject pickUpPanel;
	public GameObject picPanel;
	public GameObject itemBtn;
	public GameObject sceneLoader;
	public GameObject[] itemSpots;
	private List<string> itemCollected;
	public GameObject controlPanel;
	public GameObject dialogueManager;
	private List<string> messageList;
	private List<string> codeList;
	public Text[] controlFrontTexts;
	public Text controlBackText;
	public GameObject picBtn;
	public Text picText;
	public Text houseText;
	public bool missionCompleted;

	void Start () {
		target = transform.position;
		anim = GetComponentInChildren<Animator>();
		itemCollected = new List<string>();
		messageList = new List<string> {"ERROR: You need to find a key to initialize the console. Look around, it should be somewhere in this room.",
											"Key inserted. Console initialized successfully.",
											"Running...",
											"ERROR: Could not resolve the symbol BED. Please define BED for the console. (HINT: Let the console know what the BED looks like. In order to do this, you have to use a tool that stores what the BED looks like...)",
											"Mission completed. (\"Have you been to the OLYMPIA? It's an amazing place with all the high level technologies. People built their homes on top of the high-techs over ther and they are living a very happy life!\")"};
		codeList = new List<string> {"Waiting for initialization...",
										"Read the definition of BED from storage.",
										"While (BED.ToLeftWall < BED.ToRightWall), {keep moving right infinitely}.",
										"While (BED.ToTopWall < BED.ToBottomWall), {keep moving down infinitely}.",
										"DONE."};
		missionCompleted = false;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) 
			&& !housePanel.active 
			&& !dialogueBox.active
			&& !pickUpPanel.active
			&& !controlPanel.active
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
				if(other.gameObject.name == "Olympia"){
					houseText.text = "Enter the Olylmpia?";
				}
			}
			if(other.gameObject.tag == "Exit"){
				housePanel.SetActive(true);
			}
			if(other.gameObject.tag == "NPC"){
				if(missionCompleted == false){
					other.gameObject.GetComponent<DialogueTrigger>().triggerDialogue();
				}else if(missionCompleted == true && other.gameObject.name == "DJ"){
					dialogueBox.SetActive(true);
					dialogueName.text = "Dwayne Johnson";
					dialogueText.text = "That is perfect! Did you do it with the console? Man, I wish I never discard the technologies!";
				}
				hitName = "";
			}
			if(other.gameObject.tag == "PickUp"){
				pickUpPanel.SetActive(true);
				pickUpPanel.GetComponentInChildren<Text>().text = "You found a " + hitName + "! Pick it up?";
			}

			if(other.gameObject.name == "Bed"){
				picPanel.SetActive(true);
				if(itemCollected.Contains("Camera") && itemCollected.Contains("BEDChest") && !itemCollected.Contains("BEDPic")){
					picBtn.SetActive(true);
					picText.text = "This is the BED. Do you want to take a picture with your Camera?";
				}else if(!itemCollected.Contains("Camera") || !itemCollected.Contains("BEDChest")){
					picBtn.SetActive(false);
					picText.text = "This is the BED. You could store the picture of it in your chest for the console to read. But first, you need to find the Camera and the BED Chest.";
				}
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

	public void openControlPanel(){
		controlPanel.SetActive(true);
		if(itemCollected.Contains("Key")){
			controlBackText.text = messageList[1];
			controlBackText.color = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
			for(int i = 0; i < controlFrontTexts.Length; i ++){
				controlFrontTexts[i].text = codeList[i + 1];
			};
		}else{
			controlFrontTexts[0].text = codeList[0];
			controlBackText.text = messageList[0];
		}
	}

	public void runCode(){
		StartCoroutine(ExecuteAfterTime());
	}

	IEnumerator ExecuteAfterTime()
	{
		for(int i = 0; i < controlFrontTexts.Length; i ++){
			controlFrontTexts[i].color = new Color(255.0f/255.0f, 0f/255.0f, 0f/255.0f);
			controlBackText.text = messageList[2];
			yield return new WaitForSeconds(2);
			if(!itemCollected.Contains("BEDChest") || !itemCollected.Contains("BEDPic")){
				controlBackText.text = messageList[3];
				controlBackText.color = new Color(255.0f/255.0f, 0f/255.0f, 0f/255.0f);
				break;
			};
			if(i == 1){
				while(GameObject.Find("Bed").transform.position.x < 4.3){
					GameObject.Find("Bed").transform.position = new Vector3(GameObject.Find("Bed").transform.position.x + 1, GameObject.Find("Bed").transform.position.y, GameObject.Find("Bed").transform.position.z);
					yield return new WaitForSeconds(0.5f);
				}
			};
			if(i == 2){
				while(GameObject.Find("Bed").transform.position.y > 1.47){
					GameObject.Find("Bed").transform.position = new Vector3(GameObject.Find("Bed").transform.position.x, GameObject.Find("Bed").transform.position.y - 1, GameObject.Find("Bed").transform.position.z);
					yield return new WaitForSeconds(0.5f);
				}
			};
			if(i == (controlFrontTexts.Length - 1)){
				controlBackText.text = messageList[4];
				controlBackText.color = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
				missionCompleted = true;
			}
		}
	}

	public void closeControlPanel(){
		controlPanel.SetActive(false);
	}

	public void takePicture(){
		hitName = "BEDPic";
		for(int i = 0; i < itemSpots.Length; i++){
			if(itemSpots[i].GetComponent<Image>().sprite == null){
				itemCollected.Add(hitName);
				itemSpots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(hitName);
				GameObject.Find("Bed").GetComponent<DialogueTrigger>().triggerDialogue();
				break;
			}
		}
		closePicPanel();
		hitName = "";
	}

	public void closePicPanel(){
		picPanel.SetActive(false);
	}
}
