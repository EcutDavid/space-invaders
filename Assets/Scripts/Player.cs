using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
	public float speedX;
	public float padding;
	public GameObject missilePrefab;
	public AudioClip shootAudio;
	public AudioClip diedAudio;
	public bool died = false;
	public int lives = 3;
	PlayerLivesIndicator playerLivesIndicator;
	private Animator animator;
	private Renderer renderer;

	private float playerSafeZoneTime = 2f;
	private float playerSafeZoneTimer = float.MaxValue;

	void Start() {
		playerLivesIndicator = FindObjectOfType<PlayerLivesIndicator>();
		animator = GetComponent<Animator> ();
		renderer = GetComponent<Renderer> ();
		animator.enabled = false;
	}

	void Update () {
		if (died) {
			return;
		}
		if (playerSafeZoneTimer < playerSafeZoneTime) {
			playerSafeZoneTimer += Time.deltaTime;
			if (playerSafeZoneTimer >= playerSafeZoneTime) {
				animator.enabled = false;
				// This is a super bad way to code for sure...
				renderer.material.color = new Vector4(
					renderer.material.color.r,
					renderer.material.color.g,
					renderer.material.color.b,
					1.0f
				);
			}
		}
		if (Input.GetKey (KeyCode.Space)) {
			var exsitingPlayerMissiles = GameObject.FindGameObjectsWithTag("PlayerMissile");
			if (exsitingPlayerMissiles.Length == 0) {
				GetComponent<AudioSource> ().PlayOneShot (shootAudio);
 				Instantiate (missilePrefab, transform.position, transform.rotation);
			}
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position = new Vector3(transform.position.x - speedX * Time.deltaTime, transform.position.y, 0);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position = new Vector3(transform.position.x + speedX * Time.deltaTime, transform.position.y, 0);
		}
		var cameraOrthographicSize = Camera.main.orthographicSize;
		transform.position = new Vector3(
			Mathf.Clamp (transform.position.x, -cameraOrthographicSize + padding, cameraOrthographicSize - padding),
			transform.position.y,
			0
		);
	}

	public void reborn() {
		GetComponent<ParticleSystem> ().Stop ();
		lives = 3;
		playerLivesIndicator.reinitLives();
	}

	void OnTriggerEnter(Collider other) {
		if (died) {
			return;
		}
		if (other.tag != "PlayerMissile" && playerSafeZoneTimer >= playerSafeZoneTime) {
			Destroy (other.gameObject);
			GetComponent<AudioSource> ().PlayOneShot (diedAudio);
			if(lives == 1) {
				GetComponent<ParticleSystem> ().Play ();
				died = true;	
			} else {
				lives--;
				animator.enabled = true;
				playerSafeZoneTimer = 0;
			}
		}
	}
}
