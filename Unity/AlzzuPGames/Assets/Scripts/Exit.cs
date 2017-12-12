using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public void doExitGame() {
		Application.Quit();
	}
		
	public void closePanel(GameObject panel) {
		panel.SetActive (false);
	}

	public void openPanel(GameObject panel) {
		panel.SetActive (true);
	}

}
