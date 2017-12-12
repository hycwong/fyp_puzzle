using UnityEngine;
using System.Collections;
using UnityEngine.Networking; 
using UnityEngine.UI;
using System.Collections.Generic;


public class QuestionPieceState : NetworkBehaviour {

	[SyncVar(hook = "OnIdChanged")]
	public int PieceID;

	[SyncVar]
	public GameObject QuestionPiece;

	[SyncVar(hook = "OnTotalCorrectChange")]
	public int totalCorrectGuess;

	public List<Image> puzzleImage;

	public List<int> puzzleOrderList = new List<int> ();

	[SyncVar(hook = "OnCurrentPlayerChange")]
	public int currentPlayerID = 1;


	[SyncVar]
	public GameObject correctLabel;
	[SyncVar]
	public GameObject wrongLabel;


	void OnIdChanged (int ID) {
		Debug.Log ("OnIdChanged");
		PieceID = ID;
		QuestionPiece.GetComponent<Image>().sprite = puzzleImage [PieceID].sprite;
	}

	void OnTotalCorrectChange(int count) {
		Debug.Log ("OnCountCorrectChange");
		totalCorrectGuess = count;
	}

	void OnCurrentPlayerChange(int id) {
		Debug.Log ("OnCurrentPlayerChange , id = " + id);
		currentPlayerID = id;
		TurnBasedController.time = 15f;
	}


}
