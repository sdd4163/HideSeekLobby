using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;

public class LobbyManager : NetworkLobbyManager {
	static public LobbyManager s_Singleton;

	// Use this for initialization
	void Start () {
		if (!s_Singleton){
			s_Singleton = this;
		}
		Debug.Log("Manager Starting");

		DontDestroyOnLoad (gameObject);
	}
}
