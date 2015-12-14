using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyNetworkManager: NetworkManager
{
	
	public static NetworkClient myClient;
	
	[SerializeField]
	Button
		hostButton;
	[SerializeField]
	Button
		joinButton;
	[SerializeField]
	InputField
		usernameText;
	[SerializeField]
	InputField
		IPButton;
	[SerializeField]
	private string username;
	public string GetUsername(){return username;}

	public bool isHost;
	private short playerCount = 0;

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (isHost) {
			Debug.Log ("ETESTS");
			GameObject hider = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
			hider.name = "Hider";
			hider.AddComponent<Hider>();
			hider.GetComponent<Hider>().color = Color.black;
			hider.GetComponent<Hider>().PlayerID = playerControllerId;
			hider.GetComponent<PlayerNetStartup>().isHider = true;
			NetworkServer.AddPlayerForConnection(conn, hider, playerCount);
			isHost = false;
		} else {
			Debug.Log ("ETESTS2");
			GameObject seeker = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
			seeker.name = "Seeker";
			seeker.AddComponent<Seeker>();
			seeker.GetComponent<Seeker>().color = Color.red;
			seeker.GetComponent<Seeker>().PlayerID = playerControllerId;
			seeker.GetComponent<PlayerNetStartup>().isHider = false;
			NetworkServer.AddPlayerForConnection(conn, seeker, playerCount);
		}
		playerCount++;
	}

	public override void OnClientConnect(NetworkConnection conn){
		ClientScene.Ready(conn);
		ClientScene.AddPlayer (conn, 0);
	}


	void Start ()
	{
		print ("MyNetworkManager : Start");
	}
	
	void SetUsername()
	{
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;
		
	}
	//get input from text input fields
	void SetIPAddress ()
	{
		//string ipAddress = GameObject.Find ("txtIP").transform.FindChild ("Text").GetComponent<Text> ().text;
		NetworkManager.singleton.networkAddress = "127.0.0.1";//ipAddress;
	}
	
	void SetPort ()
	{
		NetworkManager.singleton.networkPort = 10777;
	}
	
	// button event handlers
	public void StartupHost ()
	{
		print ("clicked button, starting host (we hope)");
		isHost = true;
		SetIPAddress ();
		SetPort ();
		SetUsername ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame ()
	{
		print ("clicked button, join game");
		isHost = false;
		SetIPAddress ();
		SetPort ();
		SetUsername ();
		NetworkManager.singleton.StartClient ();
	}
	
	public void Disconnect ()
	{
		NetworkManager.singleton.StopHost ();
	}
	
	//make sure correct buttons appear on different scenes 
	void OnLevelWasLoaded (int level)
	{
		print ("Level Loaded : " + level);
		if (level == 0) {
			SetupLoginButtons ();
		} else {
			SetupChatSceneButtons ();
			//Debug.Log(playerCount);
			//ClientScene.AddPlayer (client.connection, 0);
			//Debug.Log(playerCount);
		}
	}
	
	
	//Do this in code because when we leave a scene, all objects are destroyed. 
	//Then when we return, the objects are new, and we will have lost our references.
	void SetupLoginButtons ()
	{
		print ("MyNetworkManager : SetupMenuSceneButtons");
		
		GameObject.Find ("btnHostGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnHostGame").GetComponent<Button> ().onClick.AddListener (StartupHost);
		
		GameObject.Find ("btnJoinGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnJoinGame").GetComponent<Button> ().onClick.AddListener (JoinGame);
		
	}
	
	void SetupChatSceneButtons ()
	{
		GameObject.Find ("btnDisconnect").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnDisconnect").GetComponent<Button> ().onClick.AddListener (NetworkManager.singleton.StopHost);
		
		//chat = GameObject.Find ("ChatGO").GetComponent<Chat> ();
	}
}


