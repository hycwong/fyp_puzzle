using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prototype.NetworkLobby;


public class RoomLobbyController : MonoBehaviour {

	public GameObject SharedObject2;
	public SharedVariable2 v;

	public bool AlreadySendImage = false;
	public bool sending = false;

	public Button SendImageButton;
	public GameObject SendingPanel;

	public Image previewImage;
	public GameObject previewMask;


	void Start() {
		SharedObject2 = GameObject.Find ("SharedObject2");
		v = SharedObject2.GetComponent<SharedVariable2> ();
	}


	void loadImage(byte[] data, int w, int h) {

		Texture2D t = new Texture2D (w, h, TextureFormat.BGRA32, false);
		t.LoadImage (data);

		Sprite newSprite = Sprite.Create (t as Texture2D, new Rect (0f, 0f, t.width, t.height), Vector2.zero);

		int base_w = w;
		int base_h = h;

		if (w > h) {
			base_w = 300;
			base_h = base_w * h / w;
		} else if (w < h) {
			base_h = 300;
			base_w = base_h * w / h;
		} else {
			base_w = 300;
			base_h = base_w;
		}

		previewImage.rectTransform.sizeDelta = new Vector2 (base_w, base_h);
		previewImage.sprite = newSprite;
		previewMask.SetActive (false);
	}


	void Update() {
		
		if (SharedObject2 == null) {
			SharedObject2 = GameObject.Find ("SharedObject2");
			v = SharedObject2.GetComponent<SharedVariable2> ();
		}

		if (LobbyManager.s_Singleton._playerNumber > 1 && sending == false) {
			SendImageButton.interactable = true;
			SendImageButton.GetComponentInChildren<Text> ().color = new Color32(50, 50, 50, 255);
			SendingPanel.SetActive (false);

		} else {
			SendImageButton.interactable = false;
			SendImageButton.GetComponentInChildren<Text> ().color = Color.white;
			AlreadySendImage = false;

			if (sending == true) {
				SendingPanel.SetActive (true);
			}
		}

	}



	public void SendImage() {
		if (v.imageData != null && v.width != 0 && v.height != 0) {
			PrepareForSendingImage (v.imageData, v.width, v.height);
		}
	}


	void PrepareForSendingImage(byte[] data, int w, int h) {

		GameObject[] lobbyLists = GameObject.FindGameObjectsWithTag ("PlayerInfo");
		GameObject thisPlayer = null;
		LobbyPlayer thisPlayerInfo = null;

		for (int i = 0; i < lobbyLists.Length; i++) {
			if (lobbyLists [i].GetComponent<LobbyPlayer> ().isLocalPlayer) {
				thisPlayer = lobbyLists [i];
				thisPlayerInfo = lobbyLists [i].GetComponent<LobbyPlayer> ();
				break;
			}
		}

		thisPlayerInfo.width = 0;
		thisPlayerInfo.width = w;

		thisPlayerInfo.height = 0;
		thisPlayerInfo.height = h;

		thisPlayerInfo.imageData = data;
		thisPlayerInfo.imageSize = data.Length;

		StartCoroutine (WaitAndSendImage(thisPlayerInfo, data));
	}


	IEnumerator WaitAndSendImage(LobbyPlayer thisPlayerInfo, byte[] data) {

		sending = true;

		int bufferSize = 30000;
		int time = (data.Length % bufferSize != 0) ? Mathf.FloorToInt (data.Length / bufferSize) + 1 : Mathf.FloorToInt (data.Length / bufferSize);
		Debug.Log (data.Length + " => " + time);

		byte[] first;
		if (data.Length <= bufferSize) {
			first = new byte[data.Length];
			for (int j = 0; j < data.Length; j++)
				first [j] = data [j];
			if (sending == true)
				thisPlayerInfo.RpcSendImage (first, true, true);
		} else {
			first = new byte[bufferSize];
			for (int j = 0; j < first.Length; j++)
				first [j] = data [j];
			if (sending == true)
				thisPlayerInfo.RpcSendImage (first, true, false);
		}
			
		for (int i = 1; i < time - 1; i++) {
			byte[] temp = new byte[bufferSize];
			for (int j = i * bufferSize, k = 0; j < (i + 1) * bufferSize; j++, k++)
				temp [k] = data [j];

			yield return new WaitForSeconds (0.3f);
			if (sending == true)
				thisPlayerInfo.RpcSendImage (temp, false, false);
		}

		if (data.Length > bufferSize) {
			byte[] last = new byte[data.Length - (time - 1) * bufferSize];
			for (int j = (time - 1) * bufferSize, k = 0; j < data.Length; j++, k++)
				last [k] = data [j];

			yield return new WaitForSeconds (0.3f);
			if (sending == true)
				thisPlayerInfo.RpcSendImage (last, false, true);
		}
			
		sending = false;
		AlreadySendImage = true;
		SendImageButton.GetComponentInChildren<Text> ().text = "Resend\nImage";

	}


	public void CancelSendingImage() {
		
		GameObject[] lobbyLists = GameObject.FindGameObjectsWithTag ("PlayerInfo");
		GameObject thisPlayer = null;
		LobbyPlayer thisPlayerInfo = null;

		for (int i = 0; i < lobbyLists.Length; i++) {
			if (lobbyLists [i].GetComponent<LobbyPlayer> ().isLocalPlayer) {
				thisPlayer = lobbyLists [i];
				thisPlayerInfo = lobbyLists [i].GetComponent<LobbyPlayer> ();
				break;
			}
		}

		sending = false;

		StartCoroutine (WaitForCancel (thisPlayerInfo));
	}

	IEnumerator WaitForCancel(LobbyPlayer thisPlayerInfo) {
		
		int w = v.width;
		int h = v.height;
		byte[] data = v.imageData;
		int size = v.imageSize;

		thisPlayerInfo.width = 0;
		thisPlayerInfo.height = 0;
		thisPlayerInfo.imageSize = 0;
		thisPlayerInfo.RpcSendImage (null, true, true);

		yield return new WaitForSeconds (0.2f);

		v.width = w;
		v.height = h;
		v.imageData = data;
		v.imageSize = size;
		loadImage (v.imageData, v.width, v.height);

		sending = false;
		AlreadySendImage = false;
		SendImageButton.GetComponentInChildren<Text> ().text = "Resend\nImage";
		SendingPanel.SetActive (false);

	}
		


}
