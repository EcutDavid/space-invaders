using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	public GameObject[] invadersPrefabList;
	public float padding;
	public Vector2 speed;
	public int score = 0;
	public GameObject missilePrefab;
	public AudioClip shootAudio;
	public bool playerWin = false;
	public int gameLevel = 1;
	public int gameLevelMax = 4;

	private bool movingRight = false;
	private int missileMax = 4;
	private float invadersResponseTimer = 0;
	private float invadersStartFiringSecondCount = 2f;

	void Start () {
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}

	public void invadersStopFireAWhile() {
		invadersResponseTimer = 0;
	}

	public void cleanAndRestart() {
		score = 0;
		var player = FindObjectOfType<PlayerController> ();
		player.reborn ();
		player.died = false;
		startNewLevel ();
	}

	public void startNewLevel(int level = 1) {
		Cursor.visible = false;
		invadersStopFireAWhile ();

		gameLevel = level;
		missileMax = 3 + gameLevel;
		cleanupExistingInvaders ();
		for (int i = 0; i < 11; i++) {
			for (int j = 0; j < 1; j++) {
				// As a start, assume there are just three types of invaders
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

	private void cleanupExistingInvaders() {
		var invaders = GameObject.FindGameObjectsWithTag ("Invader");
		foreach (var item in invaders) {
			Destroy (item);
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
					FindObjectOfType<PlayerController> ().died = true;
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
			var newMissileCount = missileCount - GameObject.FindGameObjectsWithTag ("InvaderMissile").Length;

			if (newMissileCount > 0) {
				GetComponent<AudioSource> ().PlayOneShot (shootAudio, 0.2f);
			}
			// This list is being used to make the missiles being fired by "random invaders"
			var attackedInvaderIndexList = new List<int> ();
			while (newMissileCount > 0) {
				var index = UnityEngine.Random.Range (0, lastRowinvaders.Count);
				if (!attackedInvaderIndexList.Exists (item => item == index)) {
					attackedInvaderIndexList.Add (index);
					newMissileCount--;
					var invader = lastRowinvaders [index];
					Instantiate(missilePrefab, invader.transform.position, invader.transform.rotation);	
				}
			}
		}
	}

	void Update ()
	{
		var invaders = GameObject.FindGameObjectsWithTag ("Invader");
		var player = FindObjectOfType<PlayerController> ();

		if (player.died && !Cursor.visible) {
			Cursor.visible = true;
			Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
		}
		var gameNotHasResult = invaders.Length > 0 && player.died == false;
		if (gameNotHasResult) {
			updateSpeedAndPosition (invaders);
			cleanUp(invaders);
			if (invadersResponseTimer > invadersStartFiringSecondCount) {
				attack (invaders);
			} else {
				invadersResponseTimer += Time.deltaTime;
			}
		}
	}
}
