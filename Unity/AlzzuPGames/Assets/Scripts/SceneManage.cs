using UnityEngine;
using System.Collections;


/* This Script is used to record the previous scene */

public static class SceneManage {

	private static string prev_scene_name;

	public static void SetPrevSceneName(string name){
		prev_scene_name = name;
	}

	public static string GetPrevSceneName(){
		return prev_scene_name;
	}

}
