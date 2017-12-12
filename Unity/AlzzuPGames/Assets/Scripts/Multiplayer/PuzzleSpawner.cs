using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;


public class PuzzleSpawner : NetworkBehaviour {

	public List<int> puzzleOrderList;
	public List<Puzzle> puzzleList;


	public override void OnStartServer() {

		MaskState[] ms = GameObject.Find ("GameSpace").GetComponentsInChildren<MaskState>(true);

		for (int i = 0; i < 16; i++) {
			GameObject mask = ms[i].gameObject;
			mask.SetActive (true);
			NetworkServer.Spawn(mask);
		}
			

		puzzleOrderList = PuzzleReady.list;

		QuestionPieceState qs = GameObject.Find ("QuestionPanel").GetComponentInChildren<QuestionPieceState>(true);
		GameObject questionPiece = qs.gameObject;

		questionPiece.SetActive (true);
		qs.PieceID = puzzleOrderList [0];
		qs.puzzleOrderList = puzzleOrderList;

		NetworkServer.Spawn(questionPiece);


	}


}
	
