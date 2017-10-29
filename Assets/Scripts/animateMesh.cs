using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateMesh : MonoBehaviour {
	public MeshFilter[] prefabList;
	private float timeCounter;
	private int meshSwithcer = 0;
	
	void Update () {
		timeCounter += Time.deltaTime;
		if (timeCounter > 1 && prefabList.Length != 0) {
			timeCounter = 0;
			meshSwithcer = meshSwithcer == 0 ? 1 : 0;
			GetComponent<MeshFilter> ().mesh = prefabList[meshSwithcer].sharedMesh;
		}
	}
}
