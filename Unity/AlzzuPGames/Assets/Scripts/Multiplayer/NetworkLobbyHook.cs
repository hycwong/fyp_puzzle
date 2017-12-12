using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

//https://www.youtube.com/watch?v=-t9kzrLkF10
public class NetworkLobbyHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, 
															GameObject lobbyPlayer, GameObject gamePlayer) {

		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer> ();
		GamePlayerController localPlayer = gamePlayer.GetComponent<GamePlayerController> ();

		localPlayer.pname = lobby.playerName;
		localPlayer.playerColor = lobby.playerColor;
		localPlayer.playerID = lobby.playerID;

	}

}
