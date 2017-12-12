using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class PuzzleClick : MonoBehaviour {	/* After Clicking a piece of puzzle */

	public static bool gameStart = false;	// Whether player clicks on the puzzle and starts the game
	private bool gameFinish = false;	// Whether game is finished
	private bool startGuess = false;	// Whether player can start next guess

	/* gameStart = false && gameFinish = false --> Game Not Start
	 * gameStart = true && gameFinish = false --> Game Starting
	 * gameStart = false && gameFinish = true --> Game Finish
	 */

	private int guessPuzzleIndex;		// ID of a puzzle that player click for guessing

	private int countGuess = 0;			// Number of guesses that player makes
	private int countCorrectGuess = 0;	// Number of correct guesses that player makes
	private int countWrongGuess = 0;	// Number of wrong guesses that player makes
	private int score = 0;				// Score that player gets

	private List<int> puzzleOrderList;	// Puzzle Order for guessing

	public List<Puzzle> puzzleList_2x2;	// Lists of all puzzle pieces
	public List<Puzzle> puzzleList_3x3;	// Lists of all puzzle pieces
	public List<Puzzle> puzzleList_4x4;	// Lists of all puzzle pieces
	public Puzzle questionPiece;		// Puzzle for guessing

	public Text guessNumber;			// Text for displaying guesses
	public Text scoreNumber;			// Text for displaying score

	public GameObject correctLabel;		// "Correct" Label 
	public GameObject wrongLabel;		// "Wrong" Label
	public GameObject completeLabel;	// "Complete" Label
	public GameObject lines_2x2;		// Lines that separate the picture
	public GameObject lines_3x3;		// Lines that separate the picture
	public GameObject lines_4x4;		// Lines that separate the picture

	public GPResult result;				// An object with class Result

	public int puzzleSize;				// Number of puzzle divided

	public bool withTips;
	private int tipsCount;
	AudioControll audioControll;


	void Awake(){
		audioControll = GameObject.Find ("AudioController").GetComponent<AudioControll> ();
	}


	// When the script starts, get "puzzleOrderList" from "PuzzleStart.list"
	void Start() {
		puzzleOrderList = PuzzleStart.list;
		puzzleSize = PuzzleStart.puzzleSize;

		withTips = PuzzleStart.withTips;
		if (withTips == true) {
			tipsCount = (int)(puzzleSize / 4);
		} else {
			tipsCount = 0;
		}
	}


	// When player clicks on the puzzle piece
	public void onClick(Puzzle piece) {

		// Start the game
		if (gameStart == false && gameFinish == false) {
			gameStart = true;
		}

		// If next guess is available, ckeck if that puzzle matches with questionPiece
		if (!startGuess) {
			startGuess = true;
			countGuess++;
			guessNumber.text = countGuess.ToString() ;

			guessPuzzleIndex = piece.ID;
			piece.guessMask.SetActive (false);

			StartCoroutine (CheckIfPuzzleMatch (guessPuzzleIndex));
		}

	}


	// Ckeck if the puzzle matches with questionPiece or not
	IEnumerator CheckIfPuzzleMatch(int guessPuzzleIndex) {

		if (guessPuzzleIndex == questionPiece.ID) {
			correctLabel.SetActive(true);
			audioControll.playAnsResult (true);

			yield return new WaitForSeconds(0.3f);
			correctGuess ();

			yield return new WaitForSeconds(0.2f);
			CheckIfGameIsFinished ();
		} else {
			wrongLabel.SetActive(true);
			audioControll.playAnsResult (false);
			yield return new WaitForSeconds(0.3f);
			wrongGuess ();
		}

		yield return new WaitForSeconds(0.3f);
		correctLabel.SetActive(false);
		wrongLabel.SetActive(false);

		if (gameStart == false && gameFinish == true) { // Game is finished
			completeLabel.SetActive (true);

			if (puzzleSize == 4) {
				lines_2x2.SetActive (false);
			} else if (puzzleSize == 9) {
				lines_3x3.SetActive (false);
			} else if (puzzleSize == 16) {
				lines_4x4.SetActive (false);
			}

			yield return new WaitForSeconds (1.3f);
			result.setAttributes (countGuess, countCorrectGuess, countWrongGuess, time, puzzleSize);
			audioControll.playSoundYay ();
			result.prepareResultTable ();
		}

		startGuess = false;

	}


	// If matching
	void correctGuess() {
		Debug.Log ("Correct");

		if (puzzleSize == 4) {
			puzzleList_2x2 [guessPuzzleIndex].puzzleButton.interactable = false;
		} else if (puzzleSize == 9) {
			puzzleList_3x3 [guessPuzzleIndex].puzzleButton.interactable = false;
		} else if (puzzleSize == 16) {
			puzzleList_4x4 [guessPuzzleIndex].puzzleButton.interactable = false;
		}

		countCorrectGuess++;

		score = score + 50;
		scoreNumber.text = score.ToString ();
	}


	// If NOT matching
	void wrongGuess() {
		Debug.Log ("Wrong");

		countWrongGuess++;

		score = score - 10;
		scoreNumber.text = score.ToString ();

		if (puzzleSize == 4) {
			puzzleList_2x2 [guessPuzzleIndex].guessMask.SetActive (true);
		} else if (puzzleSize == 9) {
			puzzleList_3x3 [guessPuzzleIndex].guessMask.SetActive (true);
		} else if (puzzleSize == 16) {
			puzzleList_4x4 [guessPuzzleIndex].guessMask.SetActive (true);
		}
	}


	// Check whether the game is finished or not
	// (All puzzles are successfully guessed)
	void CheckIfGameIsFinished() {

		if (countCorrectGuess == puzzleSize - tipsCount) {
			//Debug.Log ("GameFinished");

			if (countGuess == countCorrectGuess && countWrongGuess == 0) {
				score = score + 50;	// For making no wrong guess
			}
			if (puzzleSize == 4 && time < 31) {
				score = score + 50;	// For finishing within 30 sec (2x2)
			} else if (puzzleSize == 9 && time < 61) {
				score = score + 50; // For finishing within 1 min (3x3)
			} else if (puzzleSize == 16 && time < 121) {
				score = score + 50; // For finishing within 2 min (4x4)
			}

			scoreNumber.text = score.ToString ();
			gameStart = false;
			gameFinish = true;

		} else {
			if (puzzleSize == 4) {
				questionPiece.puzzleImage.sprite = puzzleList_2x2[puzzleOrderList[0]].spriteImage;
			} else if (puzzleSize == 9) {
				questionPiece.puzzleImage.sprite = puzzleList_3x3[puzzleOrderList[0]].spriteImage;
			} else if (puzzleSize == 16) {
				questionPiece.puzzleImage.sprite = puzzleList_4x4[puzzleOrderList[0]].spriteImage;
			}

			questionPiece.ID = puzzleOrderList[0];
			puzzleOrderList.RemoveAt (0);
		}

	}



	/* Timer */

	public Text timer;		// Text for display time
	private float time;		// time used for the game

	// Update is called once per frame
	void Update () {

		if(gameStart == true) {

			time += Time.deltaTime;

			int minutes = (int)time / 60;	// Divide the time by sixty to get the minutes
			int seconds = (int)time % 60;	// Use x mod 60 to get the seconds

			timer.text = string.Format ("{0:00} : {1:00}", minutes, seconds);

		}

	}



	/* Hint Button */

	public void hintOn (GameObject WholeImage) {
		WholeImage.SetActive (true);
	}

	public void hintOff (GameObject WholeImage) {
		WholeImage.SetActive (false);
	}



	/* Quit Message Confirmation */

	public GameObject quitMsgBox;
	private bool alreadyStart = false;

	public void openQuitMsgBox() {
		if (gameStart == true) {
			alreadyStart = true;
		}
		gameStart = false;

		quitMsgBox.SetActive (true);
	}

	public void Ans_Yes() {
		SceneManager.LoadSceneAsync("GP_MenuWithChoice", LoadSceneMode.Single);
	}

	public void Ans_No() {
		if (alreadyStart == true) {
			gameStart = true;
		}
		quitMsgBox.SetActive (false);
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
