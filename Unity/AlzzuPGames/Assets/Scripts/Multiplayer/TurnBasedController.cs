using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.UI;


public class TurnBasedController : MonoBehaviour {

	public GameObject Cover;

	public GameObject thisPlayer;
	public GamePlayerController thisPlayerInfo;

	public GameObject questionPiece;
	public QuestionPieceState questionPieceState;


	void Start() {
		StartCoroutine (StartToRemember());
		PuzzleGame.gameStart = false;
		PuzzleGame.gameFinish = false;
		PuzzleGame.startGuess = false;
		time = 15f;
	}


	public Text timer;
	public static float time = 15f;
	public RectTransform timerBar;

	public GameObject YourTurn;
	public GameObject OtherTurn;

	void Update () {

		if (PuzzleGame.gameFinish != true && endRemeberTime == true) {
			time -= Time.deltaTime;

			if (time <= 0) {
				time = 15f;
				questionPieceState.currentPlayerID = ( questionPieceState.currentPlayerID % 2 ) + 1;
				thisPlayerInfo.CmdChangeCurrentPlayer (questionPiece, questionPieceState.currentPlayerID);

				Debug.Log ("questionPieceState ID: " + questionPieceState.currentPlayerID);
				Debug.Log ("thisPlayerInfo ID: " + thisPlayerInfo.playerID);
			}

			if (questionPieceState.currentPlayerID == thisPlayerInfo.playerID) {
				Cover.SetActive (false);
				YourTurn.SetActive (true);
				OtherTurn.SetActive (false);
			} else {
				Cover.SetActive (true);
				YourTurn.SetActive (false);
				OtherTurn.SetActive (true);
			}


			int seconds = (time > 0) ? Mathf.CeilToInt(time % 60) : 0;
			timer.text = seconds.ToString();

			float length = time / 15f * 320;
			timerBar.sizeDelta = new Vector2 (length, 60);
		}

	}


	public GameObject RememberTime;
	public GameObject CountDownTimer;
	public GameObject baseImage;
	public GameObject lines_4x4;

	public bool endRemeberTime = false;

	IEnumerator StartToRemember() {
		
		RememberTime.SetActive (true);
		lines_4x4.SetActive (false);
		baseImage.SetActive (true);


		for (int j = 0; j < 10; j++) {
			string temp = (thisPlayer != null) ? thisPlayer.name : "null";
			Debug.Log ("TurnBased FindPlayer " + j + " , " + temp);

			yield return new WaitForSeconds(0.5f);

			if (thisPlayer == null) {
				
				GameObject[] playerLists = GameObject.FindGameObjectsWithTag ("networkPlayer");
				for (int i = 0; i < playerLists.Length; i++) {
					if (playerLists [i].GetComponent<GamePlayerController> ().isLocalPlayer) {
						thisPlayer = playerLists [i];
						thisPlayerInfo = playerLists [i].GetComponent<GamePlayerController> ();
						break;
					}
				}

			} else {
				break;
			}
		}


		CountDownTimer.SetActive (true);

		for (int i = 5; i > 0; i--) {
			CountDownTimer.GetComponent<Text>().text = i.ToString();

			if (thisPlayer == null) {
				GameObject[] lists = GameObject.FindGameObjectsWithTag ("networkPlayer");
				for (int j = 0; j < lists.Length; j++) {
					if (lists [i].GetComponent<GamePlayerController> ().isLocalPlayer) {
						thisPlayer = lists [i];
						thisPlayerInfo = lists [i].GetComponent<GamePlayerController> ();
						break;
					}
				}
			}

			yield return new WaitForSeconds(1f);
		}


		if (questionPiece == null) {
			questionPiece = GameObject.Find ("QuestionPiece");
			questionPieceState = questionPiece.GetComponent<QuestionPieceState> ();
		}

		if (questionPieceState.currentPlayerID == thisPlayerInfo.playerID) {
			Cover.SetActive (false);
			YourTurn.SetActive (true);
			OtherTurn.SetActive (false);
		} else {
			Cover.SetActive (true);
			YourTurn.SetActive (false);
			OtherTurn.SetActive (true);
		}

		Debug.Log ("questionPieceState Player ID: " + questionPieceState.currentPlayerID);
		Debug.Log ("thisPlayerInfo ID: " + thisPlayerInfo.playerID);
	
		baseImage.SetActive (false);
		RememberTime.SetActive (false);
		lines_4x4.SetActive (true);

		endRemeberTime = true;
	}



	public void openPanel(GameObject panel) {
		panel.SetActive (true);
	}

	public void closePanel(GameObject panel) {
		panel.SetActive (false);
	}



	/* Larger Question Piece Review */

	public GameObject LargerPieceReviewPanel;

	public void openLargerPieceReviewPanel(GameObject LargerPiece) {

		LargerPieceReviewPanel.SetActive (true);

		Image questionPieceImage = questionPiece.GetComponent<Image> ();

		float width = questionPieceImage.rectTransform.sizeDelta.x;
		float height = questionPieceImage.rectTransform.sizeDelta.y;

		if (width > height) {
			LargerPiece.GetComponent<RectTransform> ().sizeDelta = new Vector2 (600, 600 * height / width);
		} else if (width < height) {
			LargerPiece.GetComponent<RectTransform> ().sizeDelta = new Vector2 (600 * width / height, 600);
		} else {
			LargerPiece.GetComponent<RectTransform> ().sizeDelta = new Vector2 (600, 600);
		}

		LargerPiece.GetComponent<Image> ().sprite = questionPieceImage.sprite;

	}

}
