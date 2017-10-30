using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelIndicator : MonoBehaviour {
	private invadersController invadersController;
	// Use this for initialization
	void Start () {
		invadersController = FindObjectOfType<invadersController> ();
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = "Level: " + invadersController.gameLevel.ToString ();
	}
}
