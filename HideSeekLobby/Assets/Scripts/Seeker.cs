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
		Debug.Log ("Tag Attempt2");
		if (distance < 2.0f) {
			hider.gameObject.GetComponent<CharacterController>().enabled = false;
			Debug.Log("Tagged");
		} else {
			Debug.Log("Not Tagged");
		}
	}
	
	[Client]
	void TransmitTaggers () {
		Debug.Log ("Tag Attempt1");
		GameObject hider = GameObject.Find ("Hider");
		CmdCheckTag(hider, gameObject);
	}
}

