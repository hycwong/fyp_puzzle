using UnityEngine;
using System.Collections;

public class SharedVariables : MonoBehaviour {

	public byte[] imageData;
	public int width;
	public int height;
	public int level;
	public string imageFrom;

	public bool withTips;

	void Awake() {
		// Stop object from automatically destroyed on loading a scene
		DontDestroyOnLoad (this);

		if (FindObjectsOfType (GetType ()).Length > 1) {
			Destroy(GameObject.FindGameObjectsWithTag ("SharedObject")[0]);
		}

	}

}
