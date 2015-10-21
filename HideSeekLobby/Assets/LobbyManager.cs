using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;

namespace UnityStandardAssets.Network
{
	public class LobbyManager : NetworkLobbyManager {

		static public LobbyManager s_Singleton;

		protected UnityStandardAssets.Network.LobbyHook _lobbyHook;

		private int playerCount;
		public int PlayerCount {
			get {
				return playerCount;
			}
		}
	
		// Use this for initialization
		void Start () {
			s_Singleton = this;
			_lobbyHook = GetComponent<UnityStandardAssets.Network.LobbyHook> ();

			playerCount = 0;

			DontDestroyOnLoad (gameObject);
		}

		public override bool OnLobbyServerSceneLoadedForPlayer (GameObject lobbyPlayer, GameObject gamePlayer)
		{
			//This hook allows you to apply state data from the lobby-player to the game-player
			//just subclass "LobbyHook" and add it to the lobby object.

			if (_lobbyHook){
				_lobbyHook.OnLobbyServerSceneLoadedForPlayer (this, lobbyPlayer, gamePlayer);
				playerCount++;
			}
			
			return true;
		}
	}
}
