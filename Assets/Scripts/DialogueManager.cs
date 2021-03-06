﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	public Text nameText;
	public Text dialogueText;
	public GameObject dialogueBox;
	public GameObject player;

	private Queue<string> sentences;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
	}
	public void startDialogue(Dialogue dialogue){
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences){
			sentences.Enqueue(sentence);
		}

		displayNextSentence();
	}

	public void displayNextSentence(){
		if(sentences.Count == 0){
			endDialogue();
			return;
		}
		dialogueBox.SetActive(true);
		string sentence = sentences.Dequeue();
		dialogueText.text = sentence;
	}

	void endDialogue(){
		dialogueBox.SetActive(false);
		if(nameText.text == "Console"){
			player.GetComponent<PlayerMovement>().openControlPanel();
		}
	}
}
