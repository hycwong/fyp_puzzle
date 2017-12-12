using UnityEngine;
using System.Collections;

public class GameRef : MonoBehaviour {

	// Use this for initialization
	public string game;

	void Awake(){
		DontDestroyOnLoad(this);
	}
		
	public string getGame(){
		return this.game;
	}

}
