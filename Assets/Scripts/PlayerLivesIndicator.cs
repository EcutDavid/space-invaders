using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivesIndicator : MonoBehaviour {
	private bool gameStarted;
	public GameObject LivePrefab;
	PlayerController playerController;

	void Start() {
		playerController = FindObjectOfType<PlayerController> ();
	}

	public void reinitLives() {
		for (int i = 0; i < 3; i++) {
			var obj = Instantiate (LivePrefab, this.transform.position, Quaternion.identity);
			obj.transform.SetParent (this.transform);
			obj.transform.position = new Vector3 (
				obj.transform.position.x + 1.7f * i, obj.transform.position.y
			);
		}
	}

	void Update () {
		var lives = GameObject.FindGameObjectsWithTag ("PlayerLife");
		if (lives.Length > 0) {
			if (playerController.lives < lives.Length) {
				// Destory just one is enough, since player won't die two times in same frame
				Destroy (lives [0]);
			}
		}
	}
}
