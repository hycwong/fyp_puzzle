using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;
using System.Xml.Linq;


public class Level {

	[XmlAttribute("name")]
	public string name;

	[XmlElement("Frequency")]
	public int frequency;

	[XmlElement("HighestScore")]
	public int highestScore;

	[XmlElement("ShortestDuration")]
	public float shortestDuration;

	[XmlArray("TimeArray")]
	[XmlArrayItem("Time")]
	public  List<int> timeList = new List<int>();

	[XmlArray("ScoreArray")]
	[XmlArrayItem("Score")]
	public List<int> scoreList = new List<int> ();	


	[XmlArray("DateArray")]
	[XmlArrayItem("Date")]
	public List<DateTime> dateList = new List<DateTime> ();	

	public DateTime[] getDateInArray() {

		DateTime[] dateArr = new DateTime[dateList.Count];
		for (int i = 0; i < dateList.Count; i++) {

			dateArr [i] = dateList [i];
		}

		return dateArr;

	}



	public int[] getTimeInArray(){


		int[] timeArr = new int[timeList.Count];
		for (int i = 0; i < timeList.Count; i++) {
			
			timeArr [i] = timeList [i];
		}

		return timeArr;
	}


	public int[] getScoreInArray() {
		int[] scoreArr = new int[scoreList.Count];
		for (int i = 0; i < scoreList.Count; i++) {

			scoreArr [i] = scoreList [i];
		}

		return scoreArr;

	}

	public void addTimeData(int time){

		timeList.Add (time);
	}

	public void addScoreData(int score) {
		scoreList.Add (score);
	}

	public void addDateData(DateTime date) {
		dateList.Add (date);
	}



}
