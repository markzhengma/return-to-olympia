using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour {

	private RectTransform panelTransform;
	private bool showPanel;

	// Use this for initialization
	void Start () {
		panelTransform = GetComponent<RectTransform>();
		showPanel = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showOrClosePanel(){
		if(showPanel){
			showPanel = !showPanel;
			panelTransform.anchoredPosition = new Vector3(450, 0, 0);
		}else{
			showPanel = !showPanel;
			panelTransform.anchoredPosition = new Vector3(0, 0, 0);
		}
	}
}
