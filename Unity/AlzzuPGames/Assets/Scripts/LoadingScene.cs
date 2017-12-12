using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadingScene : MonoBehaviour {

	public GameObject audioControllerPrefab;


	AudioControll audioControll;
	GameObject ac;

	void Awake() {
		if(GameObject.Find("AudioController") == null) {
			ac = Instantiate (audioControllerPrefab) as GameObject;
			ac.name = "AudioController";
		}

		audioControll = GameObject.Find ("AudioController").GetComponent<AudioControll>();
	}


	public void LoadScene(string name) {

		if (SceneManager.GetActiveScene ().name.Equals ("MainMenu")) {
			audioControll.playStartGameSound ();
		}
			
		if (name.Equals ("GP_Menu")) {
			SceneManage.SetPrevSceneName ("GP_Menu");
		} else if (name.Equals ("JP_Menu")) {
			SceneManage.SetPrevSceneName ("JP_Menu");
		} else if (name.Equals ("JP_MenuWithChoice")) {
			SceneManage.SetPrevSceneName ("JP_MenuWithChoice");
		} else if (name.Equals ("GP_MenuWithChoice")) {
			SceneManage.SetPrevSceneName ("GP_MenuWithChoice");
		} else if (name.Equals ("MainMenu")) {
			SceneManage.SetPrevSceneName ("MainMenu");
			audioControll.playBackSound ();
		} 

		SceneManager.LoadSceneAsync (name, LoadSceneMode.Single);

	}


	public void playSoundBack() {
		audioControll.playBackSound ();
	}

	public void playSoundInfo() {
		audioControll.playInfoSound ();
	}


}