using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdator : MonoBehaviour {
	private GameController gameController;

	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}
	
	void Update () {
		GetComponent<Text> ().text = "Your Score: " + gameController.score.ToString ();
	}
}
