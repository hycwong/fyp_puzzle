using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;

		public static string roomName;
		public Text RoomNameDisplay;

		public Button SendImageButton;
		public Image previewImage_Room;
		public GameObject previewMask_Room;

		public GameObject SharedObject2;
		public SharedVariable2 v;

		public Button backButton;


        public void OnEnable() {
            lobbyManager.topPanel.ToggleVisibility(true);
        }


		void Awake() {
			backButton.onClick.RemoveAllListeners ();
			LoadScene ls = GameObject.Find ("Script").GetComponent<LoadScene> ();
			backButton.onClick.AddListener (() => ls.LoadScene_OutNetMenu ("MainMenu"));
			LoadingScene ls2 = GameObject.Find ("Script").GetComponent<LoadingScene> ();
			backButton.onClick.AddListener (() => ls2.playSoundBack());
		}


		void Update() {
			if (SharedObject2 == null) {
				SharedObject2 = GameObject.Find ("SharedObject2");
				v = SharedObject2.GetComponent<SharedVariable2> ();
			}
		}


        public void OnClickCreateMatchmakingGame() {
			roomName = "Room " + Random.Range(100, 1000).ToString();
			RoomNameDisplay.text = roomName;

            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                roomName,
                (uint)lobbyManager.maxPlayers,
                true,
				"", "", "", 0, 0,
				lobbyManager.OnMatchCreate);

            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);

			SendImageButton.GetComponentInChildren<Text> ().text = "Send\nImage";

			loadImage (v.imageData, v.width, v.height);
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
				
			previewImage_Room.rectTransform.sizeDelta = new Vector2 (base_w, base_h);
			previewImage_Room.sprite = newSprite;
			previewMask_Room.SetActive (false);
		}


        public void OnClickOpenServerList() {
            lobbyManager.StartMatchMaker();
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(lobbyServerList);

			previewImage_Room.sprite = null;
			previewImage_Room.rectTransform.sizeDelta = new Vector2 (300, 300);
			previewMask_Room.SetActive (true);
        }


    }
}
