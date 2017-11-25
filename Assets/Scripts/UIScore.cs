using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour {
	private GameController gameController;
	Text textComponent;

	void Start () {
		gameController = FindObjectOfType<GameController> ();
		textComponent = GetComponent<Text> ();
	}

	void Update () {
		if (gameController.gameStarted) {
			textComponent.text = "Score: " + gameController.score.ToString ();
		}
	}
}
