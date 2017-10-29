using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {
	public float speedY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (
			transform.position.x,
			transform.position.y + speedY * Time.deltaTime,
			transform.position.z
		);
		var cameraOrthographicSize = Camera.main.orthographicSize;
		if (Mathf.Abs (transform.position.y) > cameraOrthographicSize + 0.5) {
			Destroy (gameObject);
		}
	}
}
