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
	private int missileMax = 4;
	public int score = 0;
	public GameObject missilePrefab;
	public AudioClip shootAudio;
	public bool invadersWin;
	private float restartTimer = 99f;
	public bool playerWin = false;
	public int gameLevel = 1;

	// Use this for initialization
	void Start ()
	{
	}

	public void cleanAndRestart(int level = 1) {
		var invaders = GameObject.FindGameObjectsWithTag ("Invader");
		var player = FindObjectOfType<playerController> ();
		foreach (var item in invaders) {
			Destroy (item);
		}
		score = 0;
		player.repair ();
		player.died = false;
		restartTimer = 0;
	}

	public void restartGame(int level = 1) {
		gameLevel = level;
        //switch (gameLevel) {
        //case 1:
        //	missileMax = 4;
        //	break;
        //case 2: 
        //	missileMax = 8;
        //	break;
        //default:
        //	break;
        //}

        missileMax = gameLevel * 2; //so defficult to me.

        // As a start, assume there are just three types of invaders
        if (invadersPrefabList.Length == 3 && GameObject.FindGameObjectsWithTag("Invader").Length == 0) {
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
				if (Math.Abs (item.transform.position.y) > (cameraOrthographicSize + 0.4)) {
					FindObjectOfType<playerController> ().died = true;
				}
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

			if (missileCount > 0) {
				GetComponent<AudioSource> ().PlayOneShot (shootAudio, 0.2f);
			}
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
		var player = FindObjectOfType<playerController> ();

		if (invaders.Length > 0 && player.died == false && restartTimer > 1f) {
			updateSpeedAndPosition (invaders);
			cleanUp(invaders);
			attack (invaders);
		}
		if (restartTimer < 1f) {
			restartTimer += Time.deltaTime;
		} else {
			if (restartTimer > 1f && restartTimer < 2f) {
				restartTimer = 99f;
				restartGame ();
			}
		}
	}
}
