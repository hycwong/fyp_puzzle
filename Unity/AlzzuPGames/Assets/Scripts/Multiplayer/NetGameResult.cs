using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class NetGameResult : MonoBehaviour {

	public int numberOfPlayers;

	public GameObject[] players;
	public Text[] playerNames;
	public Image[] playerColors;
	public Text[] playerCorrectGuesses;
	public GameObject[] playerMeLabel;
	public GameObject[] playerWinCrown;
	public GameObject[] playerDrawHat;

	public Image player2Head;


	void Start() {

		Debug.Log ("NetGameResult Start");

		GameObject[] playerLists = GameObject.FindGameObjectsWithTag ("networkPlayer");

		numberOfPlayers = playerLists.Length;

		for (int i = 0; i < playerLists.Length; i++) {
			GamePlayerController gpc = playerLists [i].GetComponent<GamePlayerController> ();

			int pos = gpc.playerID - 1;

			players [pos].SetActive (true);

			playerNames [pos].text = gpc.pname;
			playerColors [pos].color = gpc.playerColor;
			playerCorrectGuesses [pos].text = gpc.myCorrectGuess.ToString ();

			if (gpc.isLocalPlayer)
				playerMeLabel [pos].SetActive (true);
		}

		for (int i = 0; i < playerLists.Length; i++) {
			playerLists [i].SetActive (false);
		}


		if (numberOfPlayers == 1) {
			players [1].SetActive (true);
			playerNames [1].text = PuzzleGame.pname [1];
			playerColors [1].color = PuzzleGame.pcolor [1];
			int guess = 16 - int.Parse (playerCorrectGuesses [0].text);
			playerCorrectGuesses [1].text = guess.ToString();
		}


		if (int.Parse (playerCorrectGuesses [0].text) > int.Parse (playerCorrectGuesses [1].text)) {

			playerWinCrown [0].SetActive (true);
			playerWinCrown [1].SetActive (false);

			playerDrawHat [0].SetActive (false);
			playerDrawHat [1].SetActive (false);

		} else if (int.Parse (playerCorrectGuesses [0].text) < int.Parse (playerCorrectGuesses [1].text)) {

			playerWinCrown [0].SetActive (false);
			playerWinCrown [1].SetActive (true);

			playerDrawHat [0].SetActive (false);
			playerDrawHat [1].SetActive (false);

		} else {

			playerWinCrown [0].SetActive (false);
			playerWinCrown [1].SetActive (false);

			playerDrawHat [0].SetActive (true);
			playerDrawHat [1].SetActive (true);
		}


	}


}

