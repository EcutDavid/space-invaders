using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour {
	private GameController gameController;
	void Start () {
		gameController = FindObjectOfType<GameController> ();
		gameObject.SetActive (false);
	}
	
	void Update () {
		GetComponent<Text> ().text = "Level: " + gameController.gameLevel.ToString ();
	}
}
