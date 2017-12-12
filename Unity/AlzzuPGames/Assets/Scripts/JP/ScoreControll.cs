using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// This script is used to calculate and updating score 

public class ScoreControll : MonoBehaviour {
	
	public RectTransform ResultPanel;
	public GameObject gameController;
	public Text score_Text;
	string time_in_stringformat;
	private ResultData data;
	int score = 0;
	int wrong_guesses = 0;
	int correct_guesses = 0 ;
	int score_bonus_time;
	int score_bonus_guesses;
	int score_deducted;
	public Text time_Text;
	float time;
	bool isGameEnd;
	bool updateOn;
	JigsawPuzzle puz ;
	AudioControll audioControll;

	float time_to_get_bonus = 5.0f; // average time limit per piece to get the bonus score;


	// Use this for initialization
   	public class ResultData{
		int correctGuess;
		int wrongGuess;
		string time_used;
		int scoreDeducted;
		int bonus_in_time;
		int bonus_in_accuracy;
		float time;
		int score;

		public ResultData (int correctGuess, int wrongGuess, string time_used, int scoreDeducted, int bonus_in_time, int bonus_in_accuracy, float time, int score)
   		{
   			this.correctGuess = correctGuess;
   			this.wrongGuess = wrongGuess;
   			this.time_used = time_used;
   			this.scoreDeducted = scoreDeducted;
   			this.bonus_in_time = bonus_in_time;
   			this.bonus_in_accuracy = bonus_in_accuracy;
   			this.time = time;
   			this.score = score;
   		}
   		
		public int CorrectGuess {
   			get {
   				return this.correctGuess;
   			}
   			set {
   				correctGuess = value;
   			}
   		}

   		public int WrongGuess {
   			get {
   				return this.wrongGuess;
   			}
   			set {
   				wrongGuess = value;
   			}
   		}

   		public string Time_used {
   			get {
   				return this.time_used;
   			}
   			set {
   				time_used = value;
   			}
   		}

   		public int ScoreDeducted {
   			get {
   				return this.scoreDeducted;
   			}
   			set {
   				scoreDeducted = value;
   			}
   		}

   		public int Bonus_in_time {
   			get {
   				return this.bonus_in_time;
   			}
   			set {
   				bonus_in_time = value;
   			}
   		}

   		public int Bonus_in_accuracy {
   			get {
   				return this.bonus_in_accuracy;
   			}
   			set {
   				bonus_in_accuracy = value;
   			}
   		}

   		public float Time {
   			get {
   				return this.time;
   			}
   			set {
   				time = value;
   			}
   		}

   		public int Score {
   			get {
   				return this.score;
   			}
   			set {
   				score = value;
   			}
   		}


	}


	void Awake () {
		audioControll = GameObject.Find ("AudioController").GetComponent<AudioControll> ();

		updateOn = true;
		puz = gameController.GetComponent<JigsawPuzzle> ();
		score_Text.text = score.ToString ();
		time = 0.0f;
		isGameEnd = false;
	}


	void Start () {
		
	}


	public void CorrectGuess(){
		score += 50;
		correct_guesses++;
		audioControll.playAnsResult (true);
		ScoreUpdate ();
	}


	public void WrongGuess(){
		if (score > 10) {
			score -= 5;
			score_deducted -= 5;
		}
		wrong_guesses++;

		audioControll.playBounce();

		ScoreUpdate ();
	}


	public int getCorrectGuess(){
		return correct_guesses;
	}


	public int getWrongGuess(){
		return wrong_guesses;
	}


	public string getTimeUsed(){
		return time_in_stringformat;
	}


	public ResultData getResultData(){
		return data;
	}


	public void ScoreUpdate(){
		score_Text.text = score.ToString();
	}


	void Update() {

		if (updateOn) {

			if (!isGameEnd) {
				TimerUpdate ();
			} else {
				time_in_stringformat = time_Text.text;
				data = new ResultData (correct_guesses, wrong_guesses, time_in_stringformat, score_deducted, score_bonus_time,score_bonus_guesses, time, score);
				audioControll.playSoundYay ();
				JPResult result = ResultPanel.GetComponent<JPResult> ();
			
				updateOn = false;
				Delay (1.0f);
				StartCoroutine(trial(result));
			}
		}
	}


	IEnumerator trial(JPResult result){
		yield return new WaitForSeconds(0.3f);
		result.showResultPage ();
	}


	IEnumerator Delay(float time){
		yield return new WaitForSeconds(time);
	}


	void TimerUpdate(){
		time += Time.deltaTime;

		int minutes = (int)time / 60;	// Divide the time by sixty to get the minutes
		int seconds = (int)time % 60;	// Use x mod 60 to get the seconds

		time_Text.text = string.Format ("{0:00} : {1:00}", minutes, seconds);
	}


	void TimerReset(){
		time = 0.0f;
	}


	public void CheckGameEnd() {
		if (PiecesScrollRect.pieceList.Count == 0) {
			Delay (2.0f);
			isGameEnd = true;
			CalculateScore ();
			return;
		}
	}


	public void CalculateScore(){
		int level = gameObject.GetComponent<JigsawPuzzle> ().gameLevel;
		if (time < level * level * time_to_get_bonus) {
			score = score + 100;
			score_bonus_time = 100;
		}

		if (wrong_guesses < level * 2) {
			score = score + 100;
			score_bonus_guesses = 100;
		}

		ScoreUpdate ();
	}


}
