using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Include Facebook namespace
using Facebook.Unity;
using System;

public class GPResult : MonoBehaviour {	/* For Calculating Result */

	public int TotalGuess;
	public int CorrectGuess;
	public int WrongGuess;
	public float Time;
	public int NUMBER_OF_PUZZLE;


	public void setAttributes(int tg, int cg, int wg, float t, int n){
		this.TotalGuess = tg;
		this.CorrectGuess = cg;
		this.WrongGuess = wg;
		this.Time = t;
		this.NUMBER_OF_PUZZLE = n;
	}


	public GameObject ResultTable;		// Table for diplay the result

	public Text TotalGuess_Text;
	public Text CorrectGuess_Text;
	public Text WrongGuess_Text;
	public Text TimeUsed_Text;

	public Text BasicScore_Text;
	public Text Bonus_Correct_Text;
	public Text Bonus_Time_Text;
	public Text FinalScore_Text;

	public Text Bonus_Time_Description;

	int finalScore;


	//FB SDK
    void Awake() {
        Button thatButton = GuiController.singleton.FBShareButton.GetComponent<Button>();
        thatButton.onClick.AddListener(() => FBShare());
    }

    public void FBShare() {
        var perms = new System.Collections.Generic.List<string>() { "publish_actions" };
        FB.LogInWithPublishPermissions(perms);
        string link_title = "看！我在 " + TimeUsed_Text.text + " 內完成 Guessing Puzzle，並獲得 " + finalScore + " 分呢！！";
        string link_description = "立刻下載：" + Environment.NewLine + " HKUST CSE 2017 FYP Group 60 完成品！";
        FB.ShareLink(new Uri("https://drive.google.com/file/d/0B9d3guZ4OjdvR3R1bG1PTTlsUU0/view?usp=sharing"), link_title, link_description, new Uri("http://i.imgur.com/MylNPd8.jpg"));
    }
    //FB SDK END


	public void prepareResultTable() {

		TotalGuess_Text.text = TotalGuess.ToString ();
		CorrectGuess_Text.text = CorrectGuess.ToString ();
		WrongGuess_Text.text = WrongGuess.ToString();

		int minutes = (int)Time / 60;
		int seconds = (int)Time % 60;
		TimeUsed_Text.text = string.Format ("{0:00} : {1:00}", minutes, seconds);

		int BasicScore = 50 * CorrectGuess + (-10) * WrongGuess;
		BasicScore_Text.text = BasicScore.ToString();

		int Bonus_Correct;
		if (TotalGuess == CorrectGuess && WrongGuess == 0) {
			Bonus_Correct = 50;
			Bonus_Correct_Text.text = "50";
		} else {
			Bonus_Correct = 0;
			Bonus_Correct_Text.text = "0";
		}

		int Bonus_Time;
		if (NUMBER_OF_PUZZLE == 4 && Time < 31) {
				Bonus_Time = 50;
				Bonus_Time_Text.text = "50";
		} else if (NUMBER_OF_PUZZLE == 9 && Time < 61) {
				Bonus_Time = 50;
				Bonus_Time_Text.text = "50";
		} else if (NUMBER_OF_PUZZLE == 16 && Time < 121) {
				Bonus_Time = 50;
				Bonus_Time_Text.text = "50";
		} else {
			Bonus_Time = 0;
			Bonus_Time_Text.text = "0";
		}

		if (NUMBER_OF_PUZZLE == 4) {
			Bonus_Time_Description.text = "If you can finish within 30 seconds";
		} else if (NUMBER_OF_PUZZLE == 9) {
			Bonus_Time_Description.text = "If you can finish within 1 minute";
		} else if (NUMBER_OF_PUZZLE == 16) {
			Bonus_Time_Description.text = "If you can finish within 2 minutes";
		}



		finalScore = BasicScore + Bonus_Correct + Bonus_Time;
		if (finalScore < 0) {
			finalScore = 0;
		}
		FinalScore_Text.text = (finalScore).ToString();

		ResultTable.SetActive (true);


		string path = Application.persistentDataPath + "/GuessingPuzzle.xml";

		// Update XML file
		if (NUMBER_OF_PUZZLE == 4) {
			LevelDataUpdate.UpdateEasy (finalScore, Time, path);
		} else if (NUMBER_OF_PUZZLE == 9) {
			LevelDataUpdate.UpdateNormal (finalScore, Time, path);
		} else if (NUMBER_OF_PUZZLE == 16) {
			LevelDataUpdate.UpdateHard (finalScore, Time, path);
		}


	}


}
