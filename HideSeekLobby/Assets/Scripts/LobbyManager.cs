using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;

public class LobbyManager : NetworkLobbyManager {
	static public LobbyManager s_Singleton;
	//protected UnityStandardAssets.Network.LobbyHook _lobbyHook;
	private int playerCount;
	public int PlayerCount {
		get {
			return playerCount;
		}
	}
	private GameObject[] players;

	bool hiderMade = false;

	// Use this for initialization
	void Start () {
		if (!s_Singleton){
			s_Singleton = this;
		}
		Debug.Log("Manager Starting");
		playerCount = 0;

		DontDestroyOnLoad (gameObject);
	}

	void Update(){
	}

	public override void OnLobbyServerSceneChanged(string sceneName)
	{
		Debug.Log ("changing");
		if (sceneName == "test")
		{
			players = GameObject.FindGameObjectsWithTag("Player");
			Debug.Log(players.Length);
			for (int i = 0; i < players.Length; i++)
			{
				if (i == 0)
				{
//					Debug.Log(players[i].GetComponent<PlayerNetStartup>().isHider);
//					Debug.Log(players[i].GetComponent<PlayerNetStartup>());
//					players[i].name = "Hider";
//					players[i].AddComponent<Hider>();
//					players[i].GetComponent<Hider>().PlayerID = i;
				}
				else
				{
//					players[i].name = "Seeker";
//					players[i].AddComponent<Seeker>();
//					players[i].GetComponent<Seeker>().PlayerID = i;
				}
			}
		}
	}
}
