using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;


public class GamePlayerController : NetworkBehaviour {

	[SyncVar]
	public string pname = "player";

	[SyncVar]
	public Color playerColor = Color.white;

	[SyncVar]
	public int playerID;

	[SyncVar(hook = "onMyGuessChanged")]
	public int myCorrectGuess;

	public Image iconBody;
	public Text guessText;
	public GameObject MeLabel;


	void Update() {
		
		if (!isLocalPlayer) {
			return;
		}

		QuestionPieceState qs = GameObject.Find("QuestionPanel").GetComponentInChildren<QuestionPieceState>(true);
		if (qs.totalCorrectGuess == 16 && PuzzleGame.gameFinish != true) {
			PuzzleGame.gameStart = false;
			PuzzleGame.gameFinish = true;
			StartCoroutine (CheckIfPuzzleMatch());
		}
			
	}


	public override void OnStartLocalPlayer() {

		RectTransform pos1 = GameObject.Find ("Pos1").GetComponent<RectTransform> ();
		RectTransform pos2 = GameObject.Find ("Pos2").GetComponent<RectTransform> ();

		if (playerID == 1) {
			transform.Translate (pos1.position.x, pos1.position.y, -5);
		} else if (playerID == 2) {
			transform.Translate (pos2.position.x, pos2.position.y, 0);
		}

	}


	void onMyGuessChanged(int guess) {
		myCorrectGuess = guess;
		guessText.text = myCorrectGuess.ToString ();
	}



	public const int PUZZLE_SIZE = 16;

	GameObject[] maskObjects = new GameObject[16];

	void Start() {
		Debug.Log ("GamePlayer Start");

		MaskState[] ms = GameObject.Find ("GameSpace").GetComponentsInChildren<MaskState>(true);
		for (int i = 0; i < PUZZLE_SIZE; i++) {
			GameObject mask = ms[i].gameObject;
			mask.SetActive (true);
			mask.transform.localPosition = new Vector3 (0, 0, 0);
			maskObjects [i] = mask;
		}

		GameObject[] playerLists = GameObject.FindGameObjectsWithTag ("networkPlayer");
		for (int i = 0; i < playerLists.Length; i++) {
			GamePlayerController gpc = null;
			if (playerLists [i].GetComponent<GamePlayerController> () != null) {
				gpc = playerLists [i].GetComponent<GamePlayerController> ();
				gpc.iconBody.color = gpc.playerColor;
			}
		}

	}


	[Command]
	public void CmdCloseMask(GameObject maskObj) {
		maskObj.GetComponent<MaskState> ().MaskCanSeen = false;
		maskObj.SetActive (false);
		Debug.Log ("CmdCloseMask");
	}
		
	[Command]
	public void CmdShowMask(GameObject maskObj) {
		maskObj.GetComponent<MaskState> ().MaskCanSeen = true;
		maskObj.SetActive (true);
		Debug.Log ("CmdShowMask");
	}

	[Command]
	public void CmdCloseButton(GameObject maskObj) {
		maskObj.GetComponent<MaskState> ().successGuess = true;
		maskObj.GetComponent<MaskState> ().button.GetComponent<Button> ().interactable = false;
		Debug.Log ("CmdCloseButton");
	}

	[Command]
	public void CmdChangeImage(GameObject questionPiece, int id) {
		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		if(playerID != 1)
			qs.PieceID = id;
		questionPiece.GetComponent<Image> ().sprite = qs.puzzleImage [qs.PieceID].sprite;
		Debug.Log ("CmdChangeImage");
	}

	[Command]
	public void CmdTotalCorrectChange(GameObject questionPiece, int count) {
		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		qs.totalCorrectGuess = count;
		Debug.Log ("CmdTotalCorrectChange");
	}

	[Command]
	public void CmdChangeCurrentPlayer(GameObject questionPiece, int id) {
		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		qs.currentPlayerID = id;
		Debug.Log ("CmdChangeCurrentPlayer");
	}

	[Command]
	public void CmdChangeMyCorrectGuess(int guess) {
		myCorrectGuess = guess;
		guessText.text = myCorrectGuess.ToString ();
		Debug.Log ("CmdChangeMyCorrectGuess");
	}

	[Command]
	public void CmdChangeResponseLabel(GameObject questionPiece, string label, bool active) {
		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		if (label == "correct") {
			qs.correctLabel.SetActive (active);
		} else if(label == "wrong") {
			qs.wrongLabel.SetActive (active);
		}
		Debug.Log ("CmdChangeResponseLabel");
	}


	[ClientRpc]
	public void RpcChangeResponseLabel(GameObject questionPiece, string label, bool active) {
		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		if (label == "correct") {
			qs.correctLabel.SetActive (active);
		} else if(label == "wrong") {
			qs.wrongLabel.SetActive (active);
		}
		Debug.Log ("RpcChangeResponseLabel");
	}

	[ClientRpc]
	public void RpcPrepareOrderList(GameObject questionPiece, int[] orderList) {
		QuestionPieceState qs = questionPiece.GetComponent<QuestionPieceState> ();
		for (int i = 0; i < orderList.Length; i++) {
			qs.puzzleOrderList [i] = orderList [i];
		}
		Debug.Log ("RpcPrepareOrderList");
	}



	IEnumerator CheckIfPuzzleMatch() {
		yield return new WaitForSeconds(0.3f);

		GameObject correctLabel = null;
		GameObject completeLabel = null;
		GameObject lines = null;

		Text[] responses = GameObject.Find ("Response").GetComponentsInChildren<Text> (true);
		for (int i = 0; i < responses.Length; i++) {
			if (responses [i].gameObject.name == "Correct") {
				correctLabel = responses [i].gameObject;
			} else if (responses [i].gameObject.name == "Complete") {
				completeLabel = responses [i].gameObject;
			}
		}

		Debug.Log (correctLabel.name);
		Debug.Log (completeLabel.name);

		RectTransform[] t = GameObject.Find("GameSpace").GetComponentsInChildren<RectTransform> (true);
		for (int i = 0; i < t.Length; i++) {
			if (t [i].gameObject.name == "Lines") {
				lines = t [i].gameObject;
				break;
			}
		}

		Debug.Log (lines.name);

		correctLabel.SetActive(false);
		completeLabel.SetActive (true);
		lines.SetActive (false);

		yield return new WaitForSeconds(1f);
		NetGameResult ngr = GameObject.Find("Canvas").GetComponentInChildren<NetGameResult> (true);
		GameObject resultPanel = ngr.gameObject;
		if (resultPanel.activeSelf == false) {
			resultPanel.SetActive (true);
		}
	}



}