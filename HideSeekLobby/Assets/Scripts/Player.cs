using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	protected int playerID;		//0 is hider, all others are seekers
	public int PlayerID {
		get {
			return playerID;
		}
		set {
			playerID = value;
		}
	}

	protected Transform playerTransform;
	public Transform PlayerTransform {
		get {
			return playerTransform;
		}
	}

	public int abilityID;
	//protected PlayerManager playManager;

	[SyncVar]
	public Color color;


	// Use this for initialization
	void Start () {
		playerTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("Ability Used");
			UseAbility(abilityID);
		}
	}

	void UseAbility (int itemID) {
		switch (itemID) {
			case 1:
				Debug.Log("Ability 1");
				break;
			case 2:
				Debug.Log("Ability 2");
				break;
			default:
				Debug.Log("No ability");
				break;
		}
	}

	void GainAbility (int itemID) {
		switch (itemID) {
		case 1:
			Debug.Log("Gained Ability 1");
			break;
		case 2:
			Debug.Log("Gained Ability 2");
			break;
		default:
			Debug.Log("No ability");
			break;
		}
	}
}
