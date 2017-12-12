using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

// This script is used to plot graph

public class DrawLine : MonoBehaviour {

	private LineRenderer lineRenderer;
	private float counter;
	private float distance;
	public Transform origin,x_endpoint,y_endpoint,testpt;


	private int[] randomArr;
	private int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 5, 8, 3, 6, 8, 4, 3, 1, 8, 9, 10, 3, 9, 5, 6, 2, 4, 4, 6, 8, 2 };

	public List<GameObject> dots;
	public List<int> int_list;


	public GameObject longest, shortest, mean;
	public GameObject highest, lowest, mean_for_score;
	private GameObject min_line_ToBeUsed, max_line_ToBeUsed, mean_line_ToBeUsed;

	public GameObject shortest_mark,longest_mark,mean_mark;
	public GameObject lowest_mark,highest_mark,mean_mark_for_score;
	private GameObject min_mark,max_mark,mean_value_mark;

	public GameObject dot_prefab,mark_prefab,date_prefab;

	public GameObject GraphPanel,GraphPanelForScore;
	public GameObject DetailPanel, DetailPanelForScore;
	public GameObject NoDataDisplayPanel;
	public GameObject firstRecordDate;
	public GameObject axis_time;
	public Button button_easy, button_normal, button_hard, button_last10, button_last30, button_lastall,button_allTime,button_month,button_week,button_score,button_time;
	public Text numOfRecord,numOfRecordForScore,firstplayDate,firstplayDateForScore;
	public GameObject[] mark;
	private Transform graph_transform, graph_transform_for_Score,  transform_toBeUsed;


	private float y_offset,x_offset,min_pos_y;

	private GameObject shortestMark;
	private GameObject startdate;
	private GameObject lastdate;

	private float mean_value;
	public float speed = 10.0f;
	private int numOfMark;
	private float y_axis_length,x_axis_length,y_mark_gap,y_value_offset,global_max_data;

	private int[] easyTime;
	private int[] normalTime;
	private int[] hardTime;

	private int[] easyScore;
	private int[] normalScore;
	private int[] hardScore;

	private DateTime[] easyDate;
	private DateTime[] normalDate;
	private DateTime[] hardDate;
	private DateTime play_since;
	GameObject date_title;

	public bool isTime, isScore;
	private bool isEasy, isNormal, isHard, isLast10, isLast30, isLastAll,isAllTime,isMonth,isWeek;
	DateHelper datehelper;
	AudioControll audioControll;
	int screen_h,screen_w;
	float screen_ratio;
	float game_ratio;
	float ratio_change;
	float x_ratio;
	float y_ratio;



	// Use this for initialization
	void Awake () {
		
		getData ();
	
		audioControll = GameObject.Find ("AudioController").GetComponent<AudioControll> ();

		y_ratio = (float)Screen.height / 1280;
		x_ratio = (float)Screen.width / 800; 


		isTime = true;
		isScore = false;
		isEasy = true;
		isAllTime = true;
		isWeek = false;
		isMonth = false;


		button_allTime.interactable = false; 
		button_month.interactable = true;
		button_week.interactable = true;

		button_time.interactable = false;
		button_score.interactable = true;

		min_mark = shortest_mark;
		max_mark = longest_mark;
		mean_value_mark = mean_mark;

		min_line_ToBeUsed = shortest;
		max_line_ToBeUsed = longest;
		mean_line_ToBeUsed = mean;

		datehelper = GetComponent<DateHelper> ();
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetWidth (7f, 7f);
		int_list = new List<int> (array);
		max_line_ToBeUsed.SetActive (false);
		min_line_ToBeUsed.SetActive (false);
		mean_line_ToBeUsed.SetActive (false);

		 y_axis_length = y_endpoint.position.y - origin.position.y;
		 x_axis_length = x_endpoint.position.x - origin.position.x;

		graph_transform = GraphPanel.transform;
		graph_transform_for_Score = GraphPanelForScore.transform;

		transform_toBeUsed = graph_transform;
	}


	void Start() {
		global_max_data = getMax (array);
		array = easyTime;
		showLevel (1);

		PlotLastAll();
	}

	void PlotData(int[] arrayData){

		getData ();

		if (isTime) {
			transform_toBeUsed = graph_transform;
			date_title = GameObject.Find ("date");
		} 

		if (isScore) {
			transform_toBeUsed = graph_transform_for_Score;
			date_title = GameObject.Find ("date_score");

		}
			
		numOfMark = 5;


		if (arrayData.Length == 0) {

			numOfMark = 0;
			numOfRecord.text = (arrayData.Length).ToString ();
			numOfRecordForScore.text = (arrayData.Length).ToString ();
			min_line_ToBeUsed.transform.position = new Vector3 (min_line_ToBeUsed.transform.position.x, origin.transform.position.y, min_line_ToBeUsed.transform.position.z);
			min_mark.GetComponent<TextMesh> ().text = " ";

			max_mark.transform.position = new Vector3(max_mark.transform.position.x,  origin.transform.position.y, max_mark.transform.position.z);

			max_mark.GetComponent<TextMesh> ().text =" ";
			mean_line_ToBeUsed.transform.position = new Vector3 (mean_line_ToBeUsed.transform.position.x, origin.transform.position.y, mean_line_ToBeUsed.transform.position.z);
			mean_value_mark.GetComponent<TextMesh> ().text = " ";
			OpenMessageBox ();
			return;
		}

		CloseMessageBox ();

		int max_data = getMax (arrayData);
		int min_data = getMin (arrayData);

		x_offset = x_axis_length * x_ratio  / arrayData.Length;
		y_offset = y_axis_length * y_ratio  / max_data;


		CreateDots(arrayData);

		lineRenderer.SetVertexCount (arrayData.Length);

		for (int i = 0; i < dots.Count-1; i++) {
			DrawLineFromAtoB (dots[i].transform, dots[i+1].transform,i);


			if (arrayData [i] == min_data) {
				min_pos_y = dots [i].transform.position.y;
			}
		}


		// Plot the scale of y axis 
		y_mark_gap = y_axis_length * y_ratio  / numOfMark;
		y_value_offset = max_data / numOfMark;
		mark = new GameObject[numOfMark];
		Vector3 vec = new Vector3 (axis_time.transform.position.x, y_endpoint.position.y, y_endpoint.position.z);


		for (int i = 0; i < numOfMark; i++) {

			if (i == 0) {
				mark [i] = Instantiate (mark_prefab,vec,Quaternion.identity,transform_toBeUsed) as GameObject;
				mark [i].GetComponent<TextMesh> ().text = ConvertUnit (max_data);
				mark [i].name = "MaxData";
			} else {
				Vector3 nextVec = new Vector3 (vec.x, vec.y - y_mark_gap, vec.z);
				mark[i] = Instantiate(mark_prefab,nextVec,Quaternion.identity,transform_toBeUsed) as GameObject;
				float display_value = max_data - ( (float)i * max_data / numOfMark );
				mark [i].GetComponent<TextMesh> ().text =  ConvertUnit (display_value);
				mark [i].name = "Data" + i + ": " +  ConvertUnit (display_value);
			}

			vec = mark [i].transform.position;

		}


		// Set the position of Shortest Time Line 
		float shortest_pos_y =  origin.transform.position.y + (y_axis_length * min_data * y_ratio / max_data);
		Vector3 short_vec = new Vector3 (shortest_mark.transform.position.x, shortest_pos_y, shortest_mark.transform.position.z);

		min_mark.GetComponent<TextMesh> ().text = ConvertUnit (min_data);
		min_line_ToBeUsed.transform.position = new Vector3 (min_line_ToBeUsed.transform.position.x, shortest_pos_y, min_line_ToBeUsed.transform.position.z);
		min_mark.transform.position = short_vec;

		max_mark.transform.position = new Vector3(max_mark.transform.position.x, y_endpoint.position.y, max_mark.transform.position.z);
		max_mark.GetComponent<TextMesh> ().text = ConvertUnit (max_data);

		// Set the position of Mean Time Line
		float mean_value = getMean(arrayData);

		float mean_pos_y = origin.transform.position.y + (y_axis_length * mean_value  * y_ratio/ max_data); 

		mean_line_ToBeUsed.transform.position = new Vector3 (mean_line_ToBeUsed.transform.position.x, mean_pos_y, mean_line_ToBeUsed.transform.position.z);
		mean_value_mark.GetComponent<TextMesh> ().text = ConvertUnit (mean_value);
		mean_value_mark.transform.position = new Vector3 (mean_value_mark.transform.position.x, mean_pos_y, mean_value_mark.transform.position.z);


		numOfRecord.text = (arrayData.Length).ToString ();
		numOfRecordForScore.text = (arrayData.Length).ToString ();

		UpdateDates (arrayData.Length);
	}


	public void PlotLastAll(){
		ClearGraph ();
		audioControll.playOptionSound ();
		selectNumberOfRecord ("All");

		DateTime[] date;

		if (isAllTime) {
			play_since = getFirstDate(array.Length);
			PlotData (array);
			return;
		}

		if (isMonth) {
			PlotData (sliceArrayBaseOnDate(30,array));
			return;
		}

		if (isWeek) {
			PlotData (sliceArrayBaseOnDate(7,array));
			return;
		}

	}

	public void PlotLast30(){
		ClearGraph ();
		audioControll.playOptionSound ();
		int[] arr;

		if (isAllTime) {
			arr = array;

		}

		if (isMonth) {
			arr = sliceArrayBaseOnDate (30, array);
		}

		if (isWeek) {
			arr = sliceArrayBaseOnDate (7, array);
		} else
			arr = array;

		play_since = getFirstDate(arr.Length);
		selectNumberOfRecord ("30");
		if (arr.Length <= 30) {
			PlotData (arr);
		} else {
			PlotData (GetLastXData (arr,30));
		}
	}

	public void PlotLast10(){
		audioControll.playOptionSound ();
		ClearGraph ();

		int[] arr;

		if (isAllTime) {
			arr = array;
		}

		if (isMonth) {
			arr = sliceArrayBaseOnDate (30, array);
		}

		if (isWeek) {
			arr = sliceArrayBaseOnDate (7, array);
		} else
			arr = array;
		
		play_since = getFirstDate(arr.Length);

		selectNumberOfRecord ("10");
		if (arr.Length <= 10) {
			PlotData (arr);
		} else {
			PlotData (GetLastXData (arr,10));
		}

	}


	public int[] GetSubArray(int[] original,int start,int length){
		int[] result = new int[length];
		int last = original.Length; 
		for (int i = 0; i < length; i++) {
			result [i] = original [last - length+i];
		}
		return result;
	}


	int[] GetLastXData(int [] arr,int x){
		if (arr.Length <= x) {
			return arr;
		} else {
			return GetSubArray (arr, arr.Length - x -1, x);
		}
	}


	void CreateDots(int[] arr){

		DateTime[] date = getDateArray ();
		for (int i = 0; i < arr.Length; i++) {
			
			float x = origin.position.x + (i * x_offset);
			int arrayMax = arr.Max();
			float y = y_endpoint.position.y - (( arrayMax - arr[i]) * y_offset ); 


			Vector3 vec = new Vector3 (x, y, origin.position.z);
			GameObject dot = Instantiate (dot_prefab, vec, Quaternion.identity,transform_toBeUsed) as GameObject;
			dot.name = "Dot" + i;
			dots.Add (dot);

			if (i == 0 ) { 
				Vector3 date_pos = new Vector3 (dots [i].transform.position.x, date_title.transform.position.y, date_title.transform.position.z);
				startdate = Instantiate(date_prefab, date_pos,Quaternion.identity,transform_toBeUsed) as GameObject;
				startdate.name = "Date " + i;
				startdate.GetComponent<TextMesh> ().text = (getFirstDate(arr.Length)).ToString ("MMM dd");
			}

			if (i == arr.Length-1) { 
				Vector3 date_pos = new Vector3 (dots [i].transform.position.x, date_title.transform.position.y, date_title.transform.position.z);
				lastdate = Instantiate(date_prefab, date_pos,Quaternion.identity,transform_toBeUsed) as GameObject;
				lastdate.name = "Date " + i;
				lastdate.GetComponent<TextMesh> ().text = getLastDate().ToString ("MMM dd");
			}
				
		}
	}


	void DrawLineFromAtoB(Transform start,Transform end,int index){
		
		lineRenderer.SetPosition (index, start.position);
		distance = Vector3.Distance (start.position, end.position);

		counter = 0.0f;

		if (counter < distance) {

			counter = counter + (0.1f / speed);

			float x = Mathf.Lerp (0, distance, counter);

			Vector3 pointA = start.position;
			Vector3 pointB = end.position;

			Vector3 pointAlongLine = x * Vector3.Normalize (pointB - pointA) + pointA;
			lineRenderer.SetPosition (index +1, pointAlongLine);

		}


		if (index != array.Length-1) {
			lineRenderer.SetPosition (index + 1, end.position);
		}

	}


	int getMax(int[] arr) {

		int max = 0;
		for (int i = 0; i < arr.Length; i++) {
			if (i == 0) {
				max = arr [i];
			} else if (arr [i] > max) {
				max = arr [i];
			}
		}

		return max;
	}


	int getMin(int[] arr) {

		int low = 999;
		for (int i = 0; i < arr.Length; i++) {
			if (i == 0) {
				low = arr [i];
			} else if (arr [i] <=low) {
				low = arr [i];
			}
		}

		return low;
	}


	float getMean(int[] arr){
		float mean = 0;

		for (int i = 0; i < arr.Length; i++) {
			mean += arr [i];
		}

		mean = mean / arr.Length;

		return mean;
	}


	public void ClearGraph(){
		lineRenderer.SetVertexCount (0);
		RemoveDots ();
		ClearLine ();
		ClearDate ();
	}


	void ClearDate(){
		Destroy (startdate);
		Destroy (lastdate);
		firstplayDate.text = " ";
		firstplayDateForScore.text = " ";
	}


	void ClearLine(){
		Destroy(shortestMark);
	}


	void RemoveDots(){

		if (dots!=null) {
			for (int i = 0; i < dots.Count; i++) {
				if (dots [i] != null) {
					Destroy (dots [i]);
				}
			}

			dots.Clear ();
		}
		if (mark!=null) {
			for (int j = 0; j < mark.Length; j++) {
				if (mark [j] != null) {
					Destroy (mark [j]);
				}
			}

		}
			
	}
		

	public void showLongest(){
		audioControll.playOptionSound1 ();
		if (max_line_ToBeUsed.activeSelf) {
			max_line_ToBeUsed.SetActive (false);
			max_mark.SetActive (false);
		} else {
			max_line_ToBeUsed.SetActive (true);
			max_mark.SetActive (true);
		}
	}


	public void showShortest(){
		audioControll.playOptionSound1 ();
		if (min_line_ToBeUsed.activeSelf) {
			min_line_ToBeUsed.SetActive (false);
			min_mark.SetActive (false);
		} else {
			min_line_ToBeUsed.SetActive (true);
			min_mark.SetActive (true);
		}
	}


	public void showMeanTime(){
		audioControll.playOptionSound1 ();
		if (mean_line_ToBeUsed.activeSelf) {
			mean_line_ToBeUsed.SetActive (false);
			mean_value_mark.SetActive (false);
		} else {
			mean_line_ToBeUsed.SetActive (true);
			mean_value_mark.SetActive (true);
		}
	}


	public void showLevel(int level){

		if (level == 1) {
			if (isTime) {
				array = easyTime;
			} else {
				array = easyScore;
			}

			button_easy.interactable = false;
			button_normal.interactable = true;
			button_hard.interactable = true;
			isEasy = true;
			isNormal = false;
			isHard = false;

		} else if (level == 2) {
			if (isTime) {
				array = normalTime;
			} else {
				array = normalScore;
			}

			button_easy.interactable = true;
			button_normal.interactable = false;
			button_hard.interactable = true;
			isEasy = false;
			isNormal = true;
			isHard = false;

		} else if(level == 3) {
			if (isTime) {
				array = hardTime;
			} else {
				array = hardScore;
			}

			button_easy.interactable = true;
			button_normal.interactable = true;
			button_hard.interactable = false;
			isEasy = false;
			isNormal = false;
			isHard = true;
		}

		Replot ();
	}


	public void setArrayTypeAndDifficulty(int type,int gamelevel) {
		if (type == 1) {
			if (gamelevel == 1) {
				array = easyTime;
			}

			if (gamelevel == 2) {
				array = normalTime;
			}

			if (gamelevel == 3) {
				array = hardTime;
			}
		}

		if (type == 2) {

			if (gamelevel == 1) {
				array = easyScore;
			}

			if (gamelevel == 2) {
				array = normalScore;
			}

			if (gamelevel == 3) {
				array = hardScore;
			}
		}

	}


	public void Replot() {

		if (isLast10) {
			PlotLast10 ();
		} else if(isLast30){
			PlotLast30 ();
		} else if(isLastAll){
			PlotLastAll ();
		}

	}


	/* for selecting the number of records to be shown */
	public void selectDateOfRecord(string number){

		int[] array_temp;
		if(number.Equals("All")){
			isAllTime = true;
			isMonth = false; 
			isWeek = false; 

			button_allTime.interactable = false; 
			button_month.interactable = true;
			button_week.interactable = true;

		}

		if (number.Equals("Month")) {
			isAllTime = false;
			isMonth = true; 
			isWeek = false; 

			button_allTime.interactable = true; 
			button_month.interactable = false;
			button_week.interactable = true	;
		}

		if (number.Equals("Week")) {
			isAllTime = false;
			isMonth = false; 
			isWeek = true; 

			button_allTime.interactable = true; 
			button_month.interactable = true;
			button_week.interactable = false;
		}

		Replot ();
	}


	public int[] sliceArrayBaseOnDate(int days,int[] arr){

		getData ();

		int[] after;
		DateTime[] dateArr;

		if (getDifficulty () .Equals (1)) {
			dateArr = easyDate;
		} else if (getDifficulty () .Equals (2)) {
			dateArr = normalDate;
		} else if (getDifficulty () .Equals (3)) {
			dateArr = hardDate;
		} else {
			return array;
		}

		if (dateArr == null) {
			Debug.Log ("dateArr is NULL");
		}

		if(dateArr != null) {
			
			play_since = getFirstDate (dateArr.Length);
			if(dateArr.Length < 1 ) return array;
			int cut_off_Index = datehelper.getIndexForDateWithinDays (dateArr, dateArr [0], days);

			int length = array.Length - cut_off_Index;
			after = new int[length];
			Array.Copy (array, cut_off_Index, after, 0, length);
			
			return after;
		}
		return array;
	}


	public void selectNumberOfRecord(string number){

		ColorBlock Last10Colors = button_last10.colors;
		ColorBlock Last30Colors = button_last30.colors;
		ColorBlock LastAllColors = button_lastall.colors;

		if(number.Equals("10")) {
			isLast10 = true;
			isLast30 = false; 
			isLastAll = false; 
		}
		if(number.Equals("30")) {
			isLast10 = false;
			isLast30 = true; 
			isLastAll = false; 
		}
		if(number.Equals("All")) {
			isLast10 = false;
			isLast30 = false; 
			isLastAll = true; 
		}
			
		if (isLast10) {
			
			button_last10.interactable = false; 
			button_last30.interactable = true; 
			button_lastall.interactable = true; 

			isLast30 = false;
			isLastAll = false;

		} else if (isLast30) {

			button_last10.interactable = true; 
			button_last30.interactable = false; 
			button_lastall.interactable = true; 

			isLast10 = false;
			isLastAll = false;

		} else if (isLastAll) {

			button_last10.interactable = true; 
			button_last30.interactable = true; 
			button_lastall.interactable = false; 

			isLast30 = false;
			isLast10 = false;
		}
	}


	public void BackToGameScene(){

		audioControll.playBackSound ();

		LoadingScene lc = GameObject.Find ("Script").GetComponent<LoadingScene> ();

		if (lc != null) {

			if (SceneManage.GetPrevSceneName().Equals ("GP_Menu")) {
				lc.LoadScene("GP_Menu");

			} else if (SceneManage.GetPrevSceneName().Equals ("JP_Menu")) {
				lc.LoadScene("JP_Menu");
			} else if (SceneManage.GetPrevSceneName().Equals ("JP_MenuWithChoice")) {
				lc.LoadScene("JP_MenuWithChoice");
			} else if (SceneManage.GetPrevSceneName().Equals("GP_MenuWithChoice")) {
				lc.LoadScene("GP_MenuWithChoice");
			} else {	
				lc.LoadScene("MainMenu");
			}

		} else {
			SceneManager.LoadSceneAsync ("MainMenu");
		}

	}
		

	public void SwitchDataType(int type){
		
		if (type == 1) {
			
			isTime = true;
			isScore = false;

			min_mark = shortest_mark;
			max_mark = longest_mark;
			mean_value_mark = mean_mark;

			min_line_ToBeUsed = shortest;
			max_line_ToBeUsed = longest;
			mean_line_ToBeUsed = mean;

			button_time.interactable = false;
			button_score.interactable = true;

			GraphPanel.SetActive (true);
			DetailPanel.SetActive (true);
			GraphPanelForScore.SetActive (false);
			DetailPanelForScore.SetActive (false);

		} else {
			
			isTime = false;
			isScore = true;

			min_mark = lowest_mark;
			max_mark = highest_mark;
			mean_value_mark = mean_mark_for_score;

			min_line_ToBeUsed = lowest;
			max_line_ToBeUsed = highest;
			mean_line_ToBeUsed = mean_for_score;

			button_time.interactable = true;
			button_score.interactable = false;

			GraphPanel.SetActive (false);
			DetailPanel.SetActive (false);
			GraphPanelForScore.SetActive (true);
			DetailPanelForScore.SetActive (true);

		}

		setArrayTypeAndDifficulty (type, getDifficulty());
		Replot ();

	}


	public string ConvertUnit(float value) {

		if (isTime) {
			return value.ToString("0.0") + "s";
		} else {
			return ((int)value).ToString();
		}

	}

	// retrieve the time and score array from the LevelDataLoader
	private void getData(){

		easyTime = LevelDataLoader.easyTime;
		normalTime = LevelDataLoader.normalTime;
		hardTime = LevelDataLoader.hardTime;

		easyScore = LevelDataLoader.easyScore;
		normalScore = LevelDataLoader.normalScore;
		hardScore = LevelDataLoader.hardScore;

		easyDate = LevelDataLoader.easyDate;
		normalDate= LevelDataLoader.normalDate;
		hardDate= LevelDataLoader.hardDate;
	}


	public int getDifficulty(){
		if (isEasy)
			return 1;
		else if (isNormal)
			return 2;
		else if (isHard)
			return 3;
		else
			return 4;
	}


	public void CloseMessageBox(){
		NoDataDisplayPanel.SetActive (false);
	}


	public void OpenMessageBox(){
		NoDataDisplayPanel.SetActive (true);
	}


	public DateTime[] getDateArray() {
		if (isEasy)
			return easyDate;
		else if (isNormal)
			return normalDate;
		else if (isHard)
			return hardDate;
		else {
			return null;
		}
	}


	public DateTime getFirstDate(int arrLength){

		if (arrLength == 0) {
			return default(DateTime);
		}

		if (isEasy) {
			return easyDate [easyDate.Length - arrLength ];
		}

		if (isNormal) {
			return normalDate [normalDate.Length - arrLength ];
		}

		if (isHard) {
			return hardDate [hardDate.Length - arrLength];
		}

		return default(DateTime);
	}


	public DateTime getLastDate(){

		if (isEasy) {
			return easyDate [easyDate.Length - 1];
		}

		if (isNormal) {
			return normalDate [normalDate.Length - 1];
		}

		if (isHard) {
			return hardDate [hardDate.Length - 1];
		}

		return default(DateTime);
	}


	public Font fontUsed;

	public void UpdateDates(int index){

		string start = getFirstDate (index).ToString ("MMM dd");
		string last = getLastDate().ToString ("MMM dd");

		if (start.Equals (last)) {
			startdate.GetComponent<TextMesh> ().text = getFirstDate (index).ToString ("MMM dd\n@ HH:mm");
			lastdate.GetComponent<TextMesh> ().text = getLastDate ().ToString ("MMM dd\n@ HH:mm");
			startdate.GetComponent<TextMesh> ().characterSize = 20;
			lastdate.GetComponent<TextMesh> ().characterSize = 20;
			startdate.GetComponent<TextMesh> ().alignment = TextAlignment.Center;
			lastdate.GetComponent<TextMesh> ().alignment = TextAlignment.Center;
		} else {
			startdate.GetComponent<TextMesh> ().text = getFirstDate (index).ToString ("MMM dd");
			lastdate.GetComponent<TextMesh> ().text = getLastDate().ToString ("MMM dd");
			startdate.GetComponent<TextMesh> ().characterSize = 20;
			lastdate.GetComponent<TextMesh> ().characterSize = 20;
		}

		firstplayDate.text = "from  " + getFirstDate (index).ToString ("MMM dd, yyyy  HH:mm", CultureInfo.InvariantCulture);
		firstplayDateForScore.text = "from  " + getFirstDate (index).ToString ("MMM dd, yyyy  HH:mm", CultureInfo.InvariantCulture);
	}



}
