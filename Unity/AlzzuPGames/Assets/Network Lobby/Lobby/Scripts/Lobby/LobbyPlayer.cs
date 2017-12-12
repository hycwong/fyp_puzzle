using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer {
        static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();

        public Button colorButton;
        public InputField nameInput;
        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        [SyncVar(hook = "OnMyColor")]
        public Color playerColor = Color.white;
		[SyncVar]
		public int playerID = 0;

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(220.0f / 255.0f, 220.0f / 255.0f, 220.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(255.0f/255.0f, 0.0f, 101.0f/255.0f,1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 180.0f / 255.0f, 200.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);

		public Color DisabledColor = new Color (200.0f / 255.0f, 200.0f / 255.0f, 200.0f / 255.0f, 1.0f);

		public byte[] imageData;
		[SyncVar(hook = "OnWidthChanged")]
		public int width = 0;
		[SyncVar(hook = "OnHeightChanged")]
		public int height = 0;
		[SyncVar(hook = "OnSizeChanged")]
		public int imageSize = 0;


        public override void OnClientEnterLobby() {
            base.OnClientEnterLobby();

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

            if (isLocalPlayer) {
                SetupLocalPlayer();
            } else {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
            OnMyName(playerName);
            OnMyColor(playerColor);
        }


        public override void OnStartAuthority() {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

           SetupLocalPlayer();
        }


        void ChangeReadyButtonColor(Color c) {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
			b.disabledColor = (c == TransparentColor) ? TransparentColor : DisabledColor;
            readyButton.colors = b;
        }


        void SetupOtherPlayer() {
            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }


        void SetupLocalPlayer() {
            nameInput.interactable = true;

            CheckRemoveButton();

            if (playerColor == Color.white)
                CmdColorChange();

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "Start";
			readyButton.interactable = false;

            //have to use child count of player prefab already setup as "this.slot" is not set yet
            if (playerName == "")
                CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount-1));

			playerID = LobbyPlayerList._instance.playerListContentTransform.childCount - 1;
			CmdIDChange (playerID);

            //we switch from simple name display to name input
            colorButton.interactable = true;
            nameInput.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            colorButton.onClick.RemoveAllListeners();
            colorButton.onClick.AddListener(OnColorClicked);

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

			GameObject SendPhotoButton = GameObject.Find("PhotoPreviewPanel").GetComponentInChildren<Button>(true).gameObject;
			GameObject WaitForImageText = null;
			Text[] texts = GameObject.Find ("PhotoPreviewPanel").GetComponentsInChildren<Text> (true);
			for (int i = 0; i < texts.Length; i++) {
				if (texts [i].name == "WaitForImageText") {
					WaitForImageText = texts [i].gameObject;
					break;
				}
			}
			
			if (playerID == 1) {
				SendPhotoButton.SetActive (true);
				WaitForImageText.SetActive (false);
			} else if (playerID != 1) {
				SendPhotoButton.SetActive (false);
				WaitForImageText.SetActive (true);
			}

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }


        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton() {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;
        }


        public override void OnClientReady(bool readyState) {
            if (readyState) {
                ChangeReadyButtonColor(TransparentColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY";
                textComponent.color = ReadyColor;
                readyButton.interactable = false;
                colorButton.interactable = false;
                nameInput.interactable = false;
            } else {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
				textComponent.text = isLocalPlayer ? "Start" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
                colorButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }
        }


        public void OnPlayerListChanged(int idx) { 
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }



        ///===== callback from sync var

        public void OnMyName(string newName) {
            playerName = newName;
            nameInput.text = playerName;
        }


        public void OnMyColor(Color newColor) {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;
        }


		public void OnWidthChanged(int w) {
			width = w;
			GameObject SharedObject2 = GameObject.FindGameObjectsWithTag ("SharedObject2")[0] as GameObject;
			SharedVariable2 v = SharedObject2.GetComponent<SharedVariable2> ();
			v.width = w;

			Image previewImage = GameObject.Find ("PreviewImage_Room").GetComponent<Image> ();
			if (height != 0) {
				if (width > height)
					previewImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (300, 300 * height / width);
				else if (width < height)
					previewImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (300 * width / height, 300);
				else
					previewImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (300, 300);
			} else
				previewImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (300, 300);

			Debug.Log ("OnWidthChanged");
		}


		void OnHeightChanged(int h) {
			height = h;
			GameObject SharedObject2 = GameObject.FindGameObjectsWithTag ("SharedObject2")[0] as GameObject;
			SharedVariable2 v = SharedObject2.GetComponent<SharedVariable2> ();
			v.height = h;

			Image previewImage = GameObject.Find ("PreviewImage_Room").GetComponent<Image> ();
			if (width != 0) {
				if (width > height)
					previewImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300, 300 * height / width);
				else if (width < height)
					previewImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300 * width / height, 300);
				else
					previewImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300, 300);
			} else
				previewImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (300, 300);

			Debug.Log ("OnHeightChanged");
		}


		void OnSizeChanged(int s) {
			imageSize = s;
			GameObject SharedObject2 = GameObject.FindGameObjectsWithTag ("SharedObject2")[0] as GameObject;
			SharedVariable2 v = SharedObject2.GetComponent<SharedVariable2> ();
			v.imageSize = s;
			Debug.Log ("OnSizeChanged");
		}



        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnColorClicked() {
            CmdColorChange();
        }


        public void OnReadyClicked() {
			GameObject SharedObject2 = GameObject.FindGameObjectsWithTag ("SharedObject2") [0] as GameObject;
			SharedVariable2 v = SharedObject2.GetComponent<SharedVariable2> ();

			if (v.imageData != null && v.width != 0 && v.height != 0) {
				SendReadyToBeginMessage ();

			} else {
				Debug.Log ("Not have Image");
				RectTransform[] lists = GameObject.Find ("LobbyPanel").GetComponentsInChildren<RectTransform> (true);

				if (playerID == 1) {
					GameObject ImageNotSend = null;
					for (int i = 0; i < lists.Length; i++)
						if (lists [i].gameObject.name == "ImageNotSend")
							ImageNotSend = lists [i].gameObject;
					ImageNotSend.SetActive (true);
				} else {
					GameObject WaitForImage = null;
					for (int i = 0; i < lists.Length; i++)
						if (lists [i].gameObject.name == "WaitForImage")
							WaitForImage = lists [i].gameObject;
					WaitForImage.SetActive (true);
				}
			}
        }


        public void OnNameChanged(string str) {
            CmdNameChanged(str);
        }


        public void OnRemovePlayerClick() {
            if (isLocalPlayer) {
                RemovePlayer();
            }
            else if (isServer)
                LobbyManager.s_Singleton.KickPlayer(connectionToClient);
        }


        public void ToggleJoinButton(bool enabled) {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }


        [ClientRpc]
        public void RpcUpdateCountdown(int countdown) {
			LobbyManager.s_Singleton.countdownPanel.UIText.text = countdown.ToString();
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }


        [ClientRpc]
        public void RpcUpdateRemoveButton() {
            CheckRemoveButton();
        }


		[ClientRpc]
		public void RpcSendImage(byte[] data, bool newImage, bool finish) {

			GameObject previewImage = GameObject.Find ("PreviewImage_Room");
			Image[] lists = previewImage.GetComponentsInChildren<Image> (true);
			GameObject previewMask = null;
			for (int i = 0; i < lists.Length; i++) {
				if (lists [i].gameObject.name == "Mask_Room") {
					previewMask = lists [i].gameObject;
					break;
				}
			}

			if (data == null) {
				imageData = null;
				previewMask.SetActive (true);
				return;
			}

			if (newImage == true) {
				imageData = data;
			} else {
				imageData = imageData.Concat (data).ToArray();
			}

			Debug.Log ("RpcSendImage + " + data.Length + " = " + imageData.Length);

			if (finish == true) {
				GameObject SharedObject2 = GameObject.FindGameObjectsWithTag ("SharedObject2") [0] as GameObject;
				SharedVariable2 v = SharedObject2.GetComponent<SharedVariable2> ();
				v.imageData = imageData;

				loadImage (previewImage, previewMask);
			}

		}
			

		void loadImage(GameObject previewImage, GameObject previewMask) {

			Texture2D t = new Texture2D (width, height, TextureFormat.BGRA32, false);
			t.LoadImage (imageData);
			Sprite newSprite = Sprite.Create (t as Texture2D, new Rect (0f, 0f, t.width, t.height), Vector2.zero);
			previewImage.GetComponent<Image>().sprite = newSprite;

			if (previewMask != null && previewMask.activeSelf == true)
				previewMask.SetActive (false);

			GameObject[] playerLists = GameObject.FindGameObjectsWithTag ("PlayerInfo");
			for (int i = 0; i < playerLists.Length; i++) {
				if (playerLists [i].GetComponent<LobbyPlayer> () != null) {
					LobbyPlayer p = playerLists [i].GetComponent<LobbyPlayer> ();
					p.readyButton.interactable = true;
				}
			}

		}



        //====== Server Command

        [Command]
        public void CmdColorChange() {
            int idx = System.Array.IndexOf(Colors, playerColor);

            int inUseIdx = _colorInUse.IndexOf(idx);

            if (idx < 0) idx = 0;

            idx = (idx + 1) % Colors.Length;

            bool alreadyInUse = false;

            do {
                alreadyInUse = false;
                for (int i = 0; i < _colorInUse.Count; ++i) {
                    if (_colorInUse[i] == idx) {
						//that color is already in use
                        alreadyInUse = true;
                        idx = (idx + 1) % Colors.Length;
                    }
                }
            } while (alreadyInUse);

            if (inUseIdx >= 0) {
				//if we already add an entry in the colorTabs, we change it
                _colorInUse[inUseIdx] = idx;
            } else {
				//else we add it
                _colorInUse.Add(idx);
            }

            playerColor = Colors[idx];
        }


        [Command]
        public void CmdNameChanged(string name) {
            playerName = name;
        }


		[Command]
		public void CmdIDChange(int id) {
			playerID = id;
		}


        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy() {
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

            int idx = System.Array.IndexOf(Colors, playerColor);

            if (idx < 0)
                return;

            for (int i = 0; i < _colorInUse.Count; ++i) {
                if (_colorInUse[i] == idx) {
					//that color is already in use
                    _colorInUse.RemoveAt(i);
                    break;
                }
            }
        }


    }

}
