using UnityEngine;
using System.Collections;
using UnityStandardAssets.Network;

public class PlayerLobbyHook : LobbyHook {
	public override void OnLobbyServerSceneLoadedForPlayer (UnityEngine.Networking.NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{

		LobbyPlayer l  = lobbyPlayer.GetComponent<LobbyPlayer> ();

		Player player = gamePlayer.GetComponent<Player>();
		player.PlayerID = GameObject.Find("LobbyManager").GetComponent<LobbyManager>().PlayerCount;

		if (player.PlayerID == 0){
			player.gameObject.AddComponent<Hider>();
			player.gameObject.name = "Hider";
		}
		else{
			player.gameObject.AddComponent<Seeker>();
			player.gameObject.name = "Seeker" + player.PlayerID.ToString();
		}
	}
}
