using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
	public float speedX;
	public float padding;
	public GameObject missilePrefab;

	void Start () {
		
	}
	
	void Update () {
		
		if (Input.GetKey (KeyCode.Space)) {
			var exsitingPlayerMissiles = GameObject.FindGameObjectsWithTag("PlayerMissile");
			if (exsitingPlayerMissiles.Length == 0) {
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

	void OnTriggerEnter(Collider other) {
		if (other.tag != "PlayerMissile") {
			Destroy (gameObject);
		}
	}
}
