using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderController : MonoBehaviour {
	private bool died = false;
	private Quaternion angularVelocity;
	public AudioClip diedAudio;
	public int score = 10;

	void Start () {
		angularVelocity = new Quaternion(
			Random.Range(1f, 30f),
			Random.Range(1f, 30f),
			Random.Range(1f, 30f),
			0
		);
	}
	
	void Update () {
		if (died) {
			this.transform.rotation = new Quaternion (
				this.transform.rotation.x + angularVelocity.x * Time.deltaTime,
				this.transform.rotation.y + angularVelocity.y * Time.deltaTime,
				this.transform.rotation.z + angularVelocity.z * Time.deltaTime,
				this.transform.rotation.w
			);
			this.transform.position = new Vector3 (
				this.transform.position.x,
				this.transform.position.y + 30f * Time.deltaTime,
				this.transform.position.z
			);
			// TODO: makes the invader looks smaller or add some particles?
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		// Personally, I don't like this kind of hard code style, I should figure out how to improve this part in my next Unity application
		if (collision.gameObject.tag == "PlayerMissile") {
			died = true;
			var rigidBody = GetComponent<Rigidbody> ();
			Destroy (rigidBody);
			var collider = GetComponent<MeshCollider> ();
			Destroy (collider);
			tag = "Died";
			GetComponent<AudioSource> ().PlayOneShot (diedAudio);
			FindObjectOfType<GameController> ().score += score;
			if (GameObject.FindGameObjectsWithTag ("Invader").Length == 0) {
				var gameController = FindObjectOfType<GameController> ();
				// TODO: change here to `maxGameLevel`, so there will be no 1, 2, 3, etc.
				if (gameController.gameLevel == 1) {
					gameController.restartGame (2);
				} else {
					gameController.playerWin = true;
				}
			}
		}
	}
}
