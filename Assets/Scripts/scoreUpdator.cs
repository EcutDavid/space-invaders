using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdator : MonoBehaviour {
	private GameController gameController;
	Text textComponent;

	void Start () {
		gameController = FindObjectOfType<GameController> ();
		textComponent = GetComponent<Text> ();
	}

	void Update () {
		// This is a bad way to controls the display of this UI Text
		if (gameController.score < 0) {
			return;
		}
		textComponent.text = "Score: " + gameController.score.ToString ();
	}
}
