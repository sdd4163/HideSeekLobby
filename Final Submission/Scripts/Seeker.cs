using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Seeker : Player {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//base.Update ();
		if (Input.GetMouseButtonDown (0) && isLocalPlayer) {
			TransmitTaggers ();
		}
	}
	
	[Command]
	void CmdCheckTag(GameObject hider, GameObject seeker) {
		float distance = Vector3.Distance (hider.transform.position, seeker.transform.position);
		if (distance < 3.0f) {
			//Ends game
			hider.gameObject.GetComponent<CharacterController>().enabled = false;
            GameObject.Find("ScriptManager").GetComponent<GameStates>().CmdSeekerWin();
		} else {
			Debug.Log("Not Tagged");
		}
	}
	
	[Client]
	void TransmitTaggers () {
		//Tells Server to check distance
		GameObject hider = GameObject.Find ("Hider");
		CmdCheckTag(hider, gameObject);
	}
}

