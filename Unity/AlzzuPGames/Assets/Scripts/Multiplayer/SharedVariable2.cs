using UnityEngine;
using System.Collections;

public class SharedVariable2 : MonoBehaviour {

	public byte[] imageData;
	public int imageSize;
	//public Texture2D imageTexture;
	public int width = 0;
	public int height = 0;

	void Awake() {
		// Stop object from automatically destroyed on loading a scene
		DontDestroyOnLoad (this);

		if (FindObjectsOfType (GetType ()).Length > 1) {
			Destroy(GameObject.FindGameObjectsWithTag ("SharedObject2")[0]);
		}

	}

}
