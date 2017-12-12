using UnityEngine;
using System;

namespace com.fyp.project.androidplugin {

	public class PhotoPlugin {

		public delegate void receivedImageData(string message, byte[] imageData);
		public receivedImageData delegatedImage;

		#if (UNITY_ANDROID && !UNITY_EDITOR)
		AndroidJavaObject androidObject;
		AndroidJavaObject unityObject;
		#endif

		private static PhotoPlugin pluginObject;

		private PhotoPlugin() {
			#if (UNITY_ANDROID && !UNITY_EDITOR)

			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

			AndroidJavaClass androidClass = new AndroidJavaClass("com.fyp.project.photoplugin.PhotoPlugin");
			androidObject = androidClass.GetStatic<AndroidJavaObject>("pluginObject");

			#endif
		}

		public static PhotoPlugin getInstance() {
			if (pluginObject == null) {
				pluginObject = new PhotoPlugin();
			}
			return pluginObject;
		}

		public void handleImage(string parameter) {
			if (delegatedImage == null) {
				Debug.Log("You must assign delegatedImage first");
			}
			else {
				#if (UNITY_ANDROID && !UNITY_EDITOR)
				delegatedImage(parameter, androidObject.Call<byte[]>("getPluginData"));
				cleanUp();
				#endif
			}

		}

		public void openCamera() {
			#if (UNITY_ANDROID && !UNITY_EDITOR)
			androidObject.Call("startCamera", unityObject);
			#endif
		}

		public void openGallery() {
			#if (UNITY_ANDROID && !UNITY_EDITOR)
			androidObject.Call("startGallery", unityObject);
			#endif
		}
			
		public void cleanUp() {
			#if (UNITY_ANDROID && !UNITY_EDITOR)
			androidObject.Call("cleanUp");
			#endif
		}

	}

}

