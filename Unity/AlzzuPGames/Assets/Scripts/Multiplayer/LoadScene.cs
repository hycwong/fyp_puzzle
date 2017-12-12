using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class LoadScene : MonoBehaviour {

	public void GoToOtherScene(string name) {
		SceneManager.LoadSceneAsync (name, LoadSceneMode.Single);
	}

	public void LoadScene_EndNetGame(string name) {
		LobbyManager.s_Singleton.GoBackButton ();
		SceneManager.LoadSceneAsync (name, LoadSceneMode.Single);
	}

	public void LoadScene_OutNetMenu(string name) {
		SceneManager.LoadSceneAsync (name, LoadSceneMode.Single);
		Destroy (GameObject.Find ("LobbyManager"));
		Destroy (GameObject.Find ("SharedObject2"));
	}

}