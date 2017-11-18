using UnityEngine;

public class MissileController : MonoBehaviour {
	public float speedY;

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

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Player") {
			Destroy (gameObject);
		}
	}
}
