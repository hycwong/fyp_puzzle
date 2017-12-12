using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Include Facebook namespace
using Facebook.Unity;
using System;
// This script is used to control the result panel after the game is finished
public class JPResult : MonoBehaviour {


	public GameObject resultPanel;
	public GameObject gameController;
	public Text correct_guess_count;
	public Text wrong_guess_count;
	public Text total_guess_count;

	public Text correct_guess_score;
	public Text wrong_guess_score;
	public Text time_used;
	public Text total_score;
	public Text bonus1;

	public Text bonus2;

	JigsawPuzzle puz ;

	ScoreControll sc;
	ScoreControll.ResultData result;
	int correctGuesses;
	int wrongGuesses;
	int totalGuesses;
	string timeused_in_String;
	int score_correct;
	int score_wrong;
	int score_time;
	float time;
	int bonus_acc;
	int finalScore;


	//FB SDK
    void Awake()
    {
        Button thatButton = GuiController.singleton.FBShareButton.GetComponent<Button>();
        thatButton.onClick.AddListener(() => FBShare());
    }

    public void FBShare()
    {
        var perms = new System.Collections.Generic.List<string>() { "publish_actions" };
        FB.LogInWithPublishPermissions(perms);
        string link_title = "看！我在 " + timeused_in_String + " 內完成 Jigsaw Puzzle，並獲得 " + finalScore + " 分呢！！";
        string link_description = "立刻下載：" + Environment.NewLine + " HKUST CSE 2017 FYP Group 60 完成品！";
        FB.ShareLink(new Uri("https://drive.google.com/file/d/0B9d3guZ4OjdvR3R1bG1PTTlsUU0/view?usp=sharing"), link_title, link_description, new Uri("http://imgur.com/2f8RPH0.jpg"));
    }
    //FB SDK END


	public void showResultPage(){
		getScoreDetails ();
		setScoreDetails ();
		resultPanel.SetActive (true);
	}


	public void getScoreDetails(){
		sc = gameController.GetComponent<ScoreControll> ();
		result = sc.getResultData ();
		correctGuesses = result.CorrectGuess;
		wrongGuesses = result.WrongGuess;
		totalGuesses = correctGuesses + wrongGuesses;
		timeused_in_String = result.Time_used;
		score_wrong = result.ScoreDeducted;
		score_time = result.Bonus_in_time;
		time = result.Time;
		finalScore = result.Score;
		bonus_acc = result.Bonus_in_accuracy;
	}


	public void setScoreDetails(){

		string path = Application.persistentDataPath + "/JigsawPuzzle.xml";

		puz = gameController.GetComponent<JigsawPuzzle> ();
		if (puz.gameLevel == 2) {
			LevelDataUpdate.UpdateEasy (finalScore, time, path);
		}

		if (puz.gameLevel  == 3) {
			LevelDataUpdate.UpdateNormal (finalScore, time, path);
		}

		if (puz.gameLevel  == 4) {
			LevelDataUpdate.UpdateHard (finalScore, time, path);
		}


		correct_guess_count.text = correctGuesses.ToString();
		wrong_guess_count.text = wrongGuesses.ToString() ;
		total_guess_count.text = totalGuesses.ToString();
		correct_guess_score.text = (50 * correctGuesses) .ToString("+#;-#;0");
		wrong_guess_score.text = score_wrong.ToString("+#;-#;0");
		time_used.text = timeused_in_String;
		total_score.text = finalScore.ToString ();
		bonus1.text = score_time.ToString ("+#;-#;0");
		bonus2.text = bonus_acc.ToString ("+#;-#;0");
	}


}
