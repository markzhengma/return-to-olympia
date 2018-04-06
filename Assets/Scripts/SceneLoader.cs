using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void exitHouse(){
		SceneManager.LoadScene("Main");
	}

	public void enterHouse(){
		SceneManager.LoadScene("House_1");
	}
}
