using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerManager : NetworkBehaviour {
    
    private List<Player> players;
    public List<Player> Players
    {
		get {
			return players;
		}
		set {
			players = value;
		}
	}

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		//players [0] = GameObject.Find ("Hider").GetComponent<Hider>();
		//Debug.Log (players [0].GetComponent<Hider>().PlayerID);
	}
}
