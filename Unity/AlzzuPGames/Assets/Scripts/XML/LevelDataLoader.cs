using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System;

public class LevelDataLoader : MonoBehaviour {

	public string GameName;

	public List<Text> Data; 
	public GameObject pastResult;
	public static int[] easyTime;
	public static int[] normalTime;
	public static int[] hardTime;

	public static int[] easyScore;
	public static int[] normalScore;
	public static int[] hardScore;

	public static DateTime[] easyDate;
	public static DateTime[] normalDate;
	public static DateTime[] hardDate;

	public string path;


	AudioControll audioCotroll;


	void Awake() {
		audioCotroll = GameObject.Find ("AudioController").GetComponent<AudioControll> ();

	}

	void Start() {
		if (GameName == "GP") {
			path = Application.persistentDataPath + "/GuessingPuzzle.xml";
		} else {
			path = Application.persistentDataPath + "/JigsawPuzzle.xml";
		}
	}

	public void showResult () {

		audioCotroll.playInfoSound ();

		LevelDataController i = LevelDataController.Load (path);

		int k = 0;

		foreach (Level level in i.levels) {
			if(level.name.Equals("easy")){
				easyTime = level.getTimeInArray();
				easyScore = level.getScoreInArray ();
				easyDate = level.getDateInArray ();
			}

			if(level.name.Equals("normal")){
				normalTime = level.getTimeInArray();
				normalScore = level.getScoreInArray ();
				normalDate = level.getDateInArray ();
			}

			if(level.name.Equals("hard")){
				hardTime = level.getTimeInArray();
				hardScore = level.getScoreInArray ();
				hardDate  = level.getDateInArray ();
			}


			Data [k].text = level.frequency.ToString();
			k++;

			float time;
			int minutes;
			int seconds;

			if (level.frequency == 0) {

				Data [k].text = " - ";
				k++;

				time = 0f;
				Data [k].text = " - ";

			} else {

				Data [k].text = level.highestScore.ToString();
				k++;

				time = level.shortestDuration;
				minutes = (int)time / 60;
				seconds = (int)time % 60;

				Data [k].text = string.Format ("{0:00} : {1:00}", minutes, seconds);
			}
			k++;
		}

		pastResult.SetActive (true);

		if (File.Exists (path)) {
			FileEncrypt.encrypt (path);
		}

	}


	public void closeResult() {
		audioCotroll.playBackSound ();
		pastResult.SetActive (false);
	}

	
}
