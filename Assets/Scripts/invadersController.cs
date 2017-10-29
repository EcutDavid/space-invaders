using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class invadersController : MonoBehaviour
{
	public GameObject[] invadersPrefabList;
	public float padding;
	public Vector2 speed;
	private bool movingRight = false;
	public int missileMax = 4;
	public GameObject missilePrefab;

	// Use this for initialization
	void Start ()
	{
		// As a start, assume there are just three types of invaders
		if (invadersPrefabList.Length == 3) {
			for (int i = 0; i < 11; i++) {
				for (int j = 0; j < 5; j++) {
					int prefabIndex = (int)Math.Floor ((float)((j + 1) / 2));
					Instantiate<GameObject> (
						invadersPrefabList [prefabIndex],
						new Vector3 (transform.position.x + (float)(1.5 * i), transform.position.y - (float)(j * 1.5), 0),
						Quaternion.identity,
						transform.parent
					);
				}
			}
		}
	}

	void cleanUp(GameObject[] invaders) {
		foreach (var item in invaders) {
			var cameraOrthographicSize = Camera.main.orthographicSize;
			if (Mathf.Abs(item.transform.position.y) > (cameraOrthographicSize + 2)) {
				Destroy (item);
			}
		}
	}

	void updateSpeedAndPosition(GameObject[] invaders) {
		float yInc = 0f;
		if (invaders.Length > 0) {
			var leftTopPoint = invaders [0].transform.position;
			var rightBottomPoint = invaders [0].transform.position;
			foreach (var item in invaders) {
				if (item.transform.position.x < leftTopPoint.x) {
					leftTopPoint.x = item.transform.position.x;
				}
				if (item.transform.position.y > leftTopPoint.y) {
					leftTopPoint.y = item.transform.position.y;
				}

				if (item.transform.position.x > rightBottomPoint.x) {
					rightBottomPoint.x = item.transform.position.x;
				}
				if (item.transform.position.y < rightBottomPoint.y) {
					rightBottomPoint.y = item.transform.position.y;
				}
			}
			var cameraOrthographicSize = Camera.main.orthographicSize;
			if (leftTopPoint.x < (-cameraOrthographicSize + padding) && !movingRight) {
				movingRight = !movingRight;
				yInc = speed.y;
			}
			if (rightBottomPoint.x > (cameraOrthographicSize - padding) && movingRight) {
				movingRight = !movingRight;
				yInc = speed.y;
			}

			foreach (var item in invaders) {
				item.transform.position = new Vector3 (
					item.transform.position.x + Time.deltaTime * (movingRight ? speed.x : -speed.x),
					item.transform.position.y + yInc,
					0f
				);
			}
		}
	}

	void attack(GameObject[] invaders) {
		List<GameObject> lastRowinvaders = new List<GameObject> ();
		foreach (var item in invaders) {
			if (item.tag != "Died") {
				var sameXItem = lastRowinvaders.Find (obj => obj.transform.position.x == item.transform.position.x);
				if (sameXItem == null) {
					lastRowinvaders.Add (item);
				};
				if (sameXItem != null && sameXItem.transform.position.y > item.transform.position.y) {
					lastRowinvaders.Remove (sameXItem);
					lastRowinvaders.Add (item);
				}
			}
		}
		if (lastRowinvaders.Count > 0) {
			var missileCount = Mathf.Min (missileMax, lastRowinvaders.Count);

			missileCount -= GameObject.FindGameObjectsWithTag ("InvaderMissile").Length;

			var attackedInvaderIndexList = new List<int> ();
			while (missileCount > 0) {
				var index = UnityEngine.Random.Range (0, lastRowinvaders.Count);
				if (!attackedInvaderIndexList.Exists (item => item == index)) {
					attackedInvaderIndexList.Add (index);
					missileCount--;
					var invader = lastRowinvaders [index];
					Instantiate(missilePrefab, invader.transform.position, invader.transform.rotation);	
				}
			}
		}
	}

	void Update ()
	{
		var invaders = GameObject.FindGameObjectsWithTag ("Invader");
		if (invaders.Length > 0) {
			updateSpeedAndPosition (invaders);
			cleanUp(invaders);
			attack (invaders);
		}
	}
}
