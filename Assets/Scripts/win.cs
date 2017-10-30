using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour {
	private bool restartingHandled = false;

	// Use this for initialization
	void Start () {
		reinit ();
	}

	// Update is called once per frame
	void Update () {
		var invadersController = FindObjectOfType<invadersController> ();
		if (invadersController.playerWin && !restartingHandled) {
			gameObject.transform.position = new Vector3 (
				gameObject.transform.position.x - 999f,
				gameObject.transform.position.y,
				gameObject.transform.position.z
			);
			restartingHandled = true;
		}
	}

	public void reinit() {
		gameObject.transform.position = new Vector3 (
			gameObject.transform.position.x + 999f,
			gameObject.transform.position.y,
			gameObject.transform.position.z
		);
	}
}
