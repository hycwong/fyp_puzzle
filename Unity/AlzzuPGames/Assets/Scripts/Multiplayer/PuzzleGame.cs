using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class PuzzleGame : MonoBehaviour {

	public static bool gameStart = false;
	public static bool gameFinish = false;
	public static bool startGuess = false;

	public List<Puzzle> puzzleList;

	public List<int> puzzleOrderList;

	public int guessPuzzleIndex;

	private int countGuess = 0;	
	public int countCorrectGuess = 0;
	private int countWrongGuess = 0;

	public GameObject correctLabel;
	public GameObject wrongLabel;
	public GameObject completeLabel;
	public GameObject lines;
	public GameObject resultPanel;

	public GameObject questionPiece; 
	public QuestionPieceState questionPieceState;

	public GameObject thisPlayer;
	public GamePlayerController thisPlayerInfo;


	public static string[] pname;
	public static Color[] pcolor;

	public string[] pn;
	public Color[] pc;


	void Start() {
		StartCoroutine (waitForFindPlayer ());
	}


	IEnumerator waitForFindPlayer() {
		Debug.Log ("waitForFindPlayer");

		for (int i = 0; i < 10; i++) {
			yield return new WaitForSeconds (0.5f);
			if (thisPlayer == null) {
				FindPlayer ();
			} else {
				break;
			}
		}
		yield return new WaitForSeconds (1f);
		UpdatePieceOrder ();

		yield return new WaitForSeconds (1f);
		puzzleOrderList = questionPieceState.puzzleOrderList;

		yield return new WaitForSeconds (0.5f);
		questionPiece.GetComponent<Image> ().sprite = questionPieceState.puzzleImage [puzzleOrderList [0]].sprite;


		GameObject[] playerLists = GameObject.FindGameObjectsWithTag ("networkPlayer");
		for (int i = 0; i < playerLists.Length; i++) {
			int id = playerLists [i].GetComponent<GamePlayerController> ().playerID;
			pn [id - 1] = playerLists [i].GetComponent<GamePlayerController> ().pname;
			pc [id - 1] = playerLists [i].GetComponent<GamePlayerController> ().playerColor;
		}

		pname = pn;
		pcolor = pc;
	}

	void FindPlayer() {
		
		Debug.Log ("FindPlayer");

		if (thisPlayer == null) {
			
			GameObject[] playerLists = GameObject.FindGameObjectsWithTag ("networkPlayer");
			for (int i = 0; i < playerLists.Length; i++) {
				if (playerLists [i].GetComponent<GamePlayerController> ().isLocalPlayer) {
					thisPlayer = playerLists [i];
					thisPlayerInfo = playerLists [i].GetComponent<GamePlayerController> ();
					thisPlayerInfo.MeLabel.SetActive (true);
					break;
				}
			}

		}
	}

	void UpdatePieceOrder() {
		
		if (thisPlayer != null) {
			
			if (thisPlayerInfo.playerID == 1) {
				int[] orderlist = new int[16];
				for (int i = 0; i < orderlist.Length; i++) {
					orderlist [i] = questionPieceState.puzzleOrderList [i];
				}

				Debug.Log ("RpcPrepareOrderList");
				thisPlayerInfo.RpcPrepareOrderList (questionPiece, orderlist);
			}

		}

	}




	public void clickMask(GameObject maskObj) {

		if (gameStart == false && gameFinish == false) {
			gameStart = true;
		}

		if (!startGuess) {
			startGuess = true;
			countGuess++;

			MaskState maskState = maskObj.GetComponent<MaskState> ();
			if (maskState != null) {
				maskState.MaskCanSeen = false;
				thisPlayerInfo.CmdCloseMask(maskState.gameObject);
			}

			guessPuzzleIndex = maskState.maskID;

			StartCoroutine (CheckIfPuzzleMatch (maskObj));
		}
	}


	IEnumerator CheckIfPuzzleMatch(GameObject maskObj) {

		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		
		if (guessPuzzleIndex == questionPiece.GetComponent<QuestionPieceState>().PieceID) {
			correctLabel.SetActive(true);

			if (thisPlayerInfo.playerID == 1) {
				thisPlayerInfo.RpcChangeResponseLabel (questionPiece, "correct", true);
			} else {
				thisPlayerInfo.CmdChangeResponseLabel (questionPiece, "correct", true);
			}

			yield return new WaitForSeconds(0.3f);
			correctGuess (maskObj);
			yield return new WaitForSeconds(0.2f);
			CheckIfGameIsFinished ();
		} else {
			wrongLabel.SetActive(true);

			if (thisPlayerInfo.playerID == 1) {
				thisPlayerInfo.RpcChangeResponseLabel (questionPiece, "wrong", true);
			} else {
				thisPlayerInfo.CmdChangeResponseLabel (questionPiece, "wrong", true);
			}

			yield return new WaitForSeconds(0.3f);
			wrongGuess (maskObj);
		}

		yield return new WaitForSeconds(0.3f);
		correctLabel.SetActive(false);
		wrongLabel.SetActive(false);

		if (thisPlayerInfo.playerID == 1) {
			thisPlayerInfo.RpcChangeResponseLabel(questionPiece, "correct", false);
			thisPlayerInfo.RpcChangeResponseLabel(questionPiece, "wrong", false);
		} else {
			thisPlayerInfo.CmdChangeResponseLabel(questionPiece, "correct", false);
			thisPlayerInfo.CmdChangeResponseLabel(questionPiece, "wrong", false);
		}


		if (gameStart == false && gameFinish == true) { // Game is finished
			completeLabel.SetActive (true);
			lines.SetActive (false);

			yield return new WaitForSeconds(1f);
			resultPanel.SetActive (true);
		}

		TurnBasedController.time = 0f;
		startGuess = false;
	}


	void correctGuess(GameObject maskObj) {
		Debug.Log ("Correct");

		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		qs.totalCorrectGuess = qs.totalCorrectGuess + 1;
		countCorrectGuess = qs.totalCorrectGuess;
		thisPlayerInfo.CmdTotalCorrectChange (qs.gameObject, countCorrectGuess);

		thisPlayerInfo.myCorrectGuess++;
		thisPlayerInfo.CmdChangeMyCorrectGuess (thisPlayerInfo.myCorrectGuess);

		MaskState maskState = maskObj.GetComponent<MaskState> ();
		if (maskState != null) {
			maskState.successGuess = true;
			thisPlayerInfo.CmdCloseButton (maskState.gameObject);
		}
	}


	void wrongGuess(GameObject maskObj) {
		Debug.Log ("Wrong");
		countWrongGuess++;

		MaskState maskState = maskObj.GetComponent<MaskState> ();
		if (maskState != null) {
			maskState.MaskCanSeen = true;
			thisPlayerInfo.CmdShowMask (maskState.gameObject);
		}
	}


	void CheckIfGameIsFinished() {

		if (countCorrectGuess == 16) {
			Debug.Log ("GameFinished");
			gameStart = false;
			gameFinish = true;
		} else if (countCorrectGuess < 16){
			Debug.Log ("CheckIfGameIsFinished");
			QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
			qs.PieceID = puzzleOrderList [puzzleOrderList.FindIndex (x => x == qs.PieceID) + 1];
			thisPlayerInfo.CmdChangeImage (qs.gameObject, qs.PieceID);
		}

	}


	public GameObject DisconnectPlayer;
	public Image DisconnectPlayerBody;
	public Text DisconnectPlayerGuess;

	void Update() {
		
		GameObject[] playerLists = GameObject.FindGameObjectsWithTag ("networkPlayer");
		int numberOfPlayers = playerLists.Length;

		// Client drop connection
		if (numberOfPlayers == 1) {
			if (DisconnectPlayer.activeSelf == false) {
				DisconnectPlayer.SetActive (true);
				DisconnectPlayerBody.color = pc [1];
				int guess = countCorrectGuess - playerLists [0].GetComponent<GamePlayerController> ().myCorrectGuess;
				DisconnectPlayerGuess.text = guess.ToString ();
			}
		} else {
			DisconnectPlayer.SetActive (false);
		}

		// Auto Guess
		if (gameStart == true && gameFinish == false) {
			
			if (numberOfPlayers == 1 && questionPieceState.currentPlayerID == 2) {

				int randomTime = 12;

				if (TurnBasedController.time <= randomTime) {
					QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
					qs.totalCorrectGuess = qs.totalCorrectGuess + 1;
					countCorrectGuess = qs.totalCorrectGuess;
					thisPlayerInfo.CmdTotalCorrectChange (qs.gameObject, countCorrectGuess);

					string name = "GuessMask (" + qs.PieceID + ")";
					GameObject maskObj = GameObject.Find (name);

					MaskState maskState = maskObj.GetComponent<MaskState> ();
					if (maskState != null) {
						maskState.MaskCanSeen = false;
						thisPlayerInfo.CmdCloseMask (maskState.gameObject);
						maskState.successGuess = true;
						thisPlayerInfo.CmdCloseButton (maskState.gameObject);

						int guess = countCorrectGuess - playerLists [0].GetComponent<GamePlayerController> ().myCorrectGuess;
						DisconnectPlayerGuess.text = guess.ToString ();
					}

					CheckIfGameIsFinished ();

					if (gameStart == false && gameFinish == true) {
						completeLabel.SetActive (true);
						lines.SetActive (false);
						resultPanel.SetActive (true);
					}

					startGuess = false;
					TurnBasedController.time = 0.0f;
				}
			}
		}


	}


	/* Hint Button */

	public void hintOn (GameObject WholeImage) {
		WholeImage.SetActive (true);
	}

	public void hintOff (GameObject WholeImage) {
		WholeImage.SetActive (false);
	}


}
