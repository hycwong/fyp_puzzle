using UnityEngine;
using System.Collections;
using com.fyp.project.androidplugin;

public class PhotoPluginCallBack : MonoBehaviour {

	// 2 callback functions that will be called in Unity by Android Plugin, 
	// with the width and height of image passed as its input parameters

	void Camera_Done(string parameter) {
		PhotoPlugin.getInstance().handleImage(parameter);
	}

	void Gallery_Done(string parameter) {
		PhotoPlugin.getInstance().handleImage(parameter);
	}

}
