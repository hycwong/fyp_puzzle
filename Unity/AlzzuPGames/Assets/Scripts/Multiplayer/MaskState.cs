using UnityEngine;
using System.Collections;
using UnityEngine.Networking; 
using UnityEngine.UI;

public class MaskState : NetworkBehaviour {

	public int maskID;

	[SyncVar(hook = "OnMaskClosed")]
	public bool MaskCanSeen = true;

	[SyncVar]
	public GameObject mask;

	[SyncVar]
	public GameObject button;

	[SyncVar(hook = "OnButtonClosed")]
	public bool successGuess = false;



	void OnMaskClosed (bool seen) {
		Debug.Log ("OnMaskClosed setActive = " + seen);
		MaskCanSeen = seen;
		mask.SetActive (seen);
	}

	void OnButtonClosed (bool close) {
		Debug.Log ("OnButtonClosed");
		if (close == true) {
			successGuess = true;
			button.GetComponent<Button> ().interactable = false;
		}
	}


}
