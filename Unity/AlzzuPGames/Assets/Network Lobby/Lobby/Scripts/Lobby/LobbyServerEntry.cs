using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

namespace Prototype.NetworkLobby {
	
    public class LobbyServerEntry : MonoBehaviour {
		
        public Text roomInfoText;
        public Text stateInfo;
        public Button joinButton;


		public void Populate(MatchInfoSnapshot match, LobbyManager lobbyManager, Color c) {
            roomInfoText.text = match.name;

            stateInfo.text = match.currentSize.ToString() + "/" + match.maxSize.ToString(); ;

            NetworkID networkID = match.networkId;

            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(() => { JoinMatch(networkID, lobbyManager); });

            GetComponent<Image>().color = c;
        }


        void JoinMatch(NetworkID networkID, LobbyManager lobbyManager) {
			lobbyManager.matchMaker.JoinMatch(networkID, "", "", "", 0, 0, lobbyManager.OnMatchJoined);
			lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();

			Text[] lists = GameObject.Find("LobbyManager").GetComponentsInChildren<Text>(true);
			Text LobbyRoomName;
			for (int i = 0; i < lists.Length; i++) {
				if(lists[i].gameObject.name=="RoomNameDisplay") {
					LobbyRoomName = lists [i];
					LobbyRoomName.text = roomInfoText.text;
					break;
				}
			}

        }


    }

}