using UnityEngine;
using System.Collections;
using System;

public class LevelDataUpdate : MonoBehaviour {


	public static void UpdateEasy(int score, float time, string path) {
		
		LevelDataController i = LevelDataController.Load (path);

		foreach (Level level in i.levels) {

			if (level.name.Equals ("easy")) {

				if (score < 0) {
					score = 0;
				}

				level.frequency = level.frequency + 1;

				if (level.highestScore < score) {
					level.highestScore = score;
				}

				if (level.shortestDuration > time) {
					level.shortestDuration = time;
				}
					
				level.addTimeData ((int)time);
				level.addScoreData (score);
				level.addDateData (DateTime.Now);
			}

		}

		i.Save (path);

	}


	public static void UpdateNormal(int score, float time, string path) {

		LevelDataController i = LevelDataController.Load (path);

		foreach (Level level in i.levels) {

			if (level.name.Equals ("normal")) {

				if (score < 0) {
					score = 0;
				}

				level.frequency = level.frequency + 1;

				if (level.highestScore < score) {
					level.highestScore = score;
				}

				if (level.shortestDuration > time) {
					level.shortestDuration = time;
				}

				level.addTimeData ((int)time);
				level.addScoreData (score);
				level.addDateData (DateTime.Now);
			}

		}

		i.Save (path);

	}


	public static void UpdateHard(int score, float time, string path) {
		
		LevelDataController i = LevelDataController.Load (path);

		foreach (Level level in i.levels) {

			if (level.name.Equals ("hard")) {

				if (score < 0) {
					score = 0;
				}

				level.frequency = level.frequency + 1;

				if (level.highestScore < score) {
					level.highestScore = score;
				}

				if (level.shortestDuration > time) {
					level.shortestDuration = time;
				}

				level.addTimeData ((int)time);
				level.addScoreData (score);
				level.addDateData (DateTime.Now);
			}

		}

		i.Save (path);

	}


}
