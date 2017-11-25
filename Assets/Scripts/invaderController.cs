using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderController : MonoBehaviour {
	private bool died = false;
	private Vector3 angularVelocity;
	public AudioClip diedAudio;
	public int score = 10;
	public ParticleSystem Particles;

	private float diedSpeedY = 12f;

	void Start () {
		angularVelocity = new Vector3(
			Random.Range(10f, 30f),
			Random.Range(3f, 9f),
			Random.Range(3f, 9f)
		);
	}
	
	void Update () {
		if (died) {
			var angles = transform.rotation.eulerAngles;
			angles.x += angularVelocity.x;
			angles.y += angularVelocity.y;
			angles.z += angularVelocity.z;
			transform.rotation = Quaternion.Euler(angles);

			this.transform.position = new Vector3 (
				this.transform.position.x,
				this.transform.position.y + diedSpeedY * Time.deltaTime,
				this.transform.position.z
			);
			// TODO: makes the invader looks smaller or add some particles?
			var cameraOrthographicSize = Camera.main.orthographicSize;
			if (System.Math.Abs (transform.position.y) > (cameraOrthographicSize + 0.8)) {
				Destroy (this.gameObject);
			}
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
			var particles = Instantiate (Particles);
			particles.transform.position = transform.position;
			GetComponent<AudioSource> ().PlayOneShot (diedAudio);
			FindObjectOfType<GameController> ().score += score;
			if (GameObject.FindGameObjectsWithTag ("Invader").Length == 0) {
				var gameController = FindObjectOfType<GameController> ();
				// TODO: change here to `maxGameLevel`, so there will be no 1, 2, 3, etc.
				if (gameController.gameLevel < gameController.gameLevelMax) {
					gameController.startNewLevel (gameController.gameLevel + 1);
				} else {
					gameController.playerWin = true;
				}
			}
		}
	}
}
