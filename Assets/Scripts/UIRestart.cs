using UnityEngine;

public class UIRestart : MonoBehaviour {
	private bool restartingHandled = false;
	private bool reinited = false;

	void Start () {
		reinit ();
	}
	
	void Update () {
		var player = FindObjectOfType<Player> ();
		if (player.died && !restartingHandled) {
			gameObject.transform.position = new Vector3 (
				gameObject.transform.position.x - 999f,
				gameObject.transform.position.y,
				gameObject.transform.position.z
			);
			reinited = false;
			restartingHandled = true;
		}
	}

	public void reinit() {
		if (reinited == false) {
			gameObject.transform.position = new Vector3 (
				gameObject.transform.position.x + 999f,
				gameObject.transform.position.y,
				gameObject.transform.position.z
			);
			reinited = true; 
			restartingHandled = false;
		}
	}
}
