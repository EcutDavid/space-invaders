using UnityEngine;

public class Win : MonoBehaviour {
	private bool restartingHandled = false;

	void Start () {
		reinit ();
	}

	void Update () {
		var invadersController = FindObjectOfType<GameController> ();
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
